using System;
using System.IO;
using System.Linq;
using Pinax;
using Pinax.Models;
using Pinax.Services;
using Xunit;

namespace Test.Pinax.Services;

public class TestProjectParser
{
    private readonly DotNetVersions _dotNetVersions =
        new()
        {
            Standard = new Version(2, 0),
            Core = new Version(3, 1),
            Framework = new Version(4, 8),
            DotNet = new Version(6,0)
        };

    [Fact]
    public void Test_GetVersionNumber_SudokuSolver()
    {
        var projectFileText =
            File.ReadAllLines("./TestFiles/ProjectFiles/SudokuSolver.txt");

        var project = 
            ProjectParser.GetProject("SudokuSolver.csproj", projectFileText, _dotNetVersions);

        Assert.NotNull(project.ProjectTypes
            .FirstOrDefault(p => p.Type == Enums.DotNetType.Framework &&
                                 p.Version.Major == 4 &&
                                 p.Version.Minor == 5));
    }

    [Fact]
    public void Test_GetProject_DocVaultDAL()
    {
        var projectFileText =
            File.ReadAllLines("./TestFiles/ProjectFiles/DocVaultDAL.txt");

        var project =
            ProjectParser.GetProject("DocVaultDAL.csproj", projectFileText, _dotNetVersions);

        Assert.NotNull(project.ProjectTypes
            .FirstOrDefault(p => p.Type == Enums.DotNetType.DotNet &&
                                 p.Version.Major == 5 &&
                                 p.Version.Minor == 0));
        Assert.Equal(4, project.Packages.Count);
    }

    [Fact]
    public void Test_GetProject_GenerativeArt()
    {
        var projectFileText =
            File.ReadAllLines("./TestFiles/ProjectFiles/MicroSiteMakerServices.txt");

        var project =
            ProjectParser.GetProject("MicroSiteMakerServices.csproj", projectFileText, _dotNetVersions);

        Assert.NotNull(project.ProjectTypes
            .FirstOrDefault(p => p.Type == Enums.DotNetType.DotNet &&
                                 p.Version.Major == 6 &&
                                 p.Version.Minor == 0));
        Assert.Equal(1, project.Packages.Count);
        Assert.Equal("Markdig", project.Packages[0].Name);
        Assert.Equal(new Version(0, 28,1), project.Packages[0].Version);
    }
}