using System.IO;
using Pinax.Models;
using Pinax.Services;
using Xunit;

namespace Test.Pinax.Services;

public class TestProjectParser
{
    [Fact]
    public void Test_GetVersionNumber_SudokuSolver()
    {
        var projectFileText =
            File.ReadAllLines("./TestFiles/ProjectFiles/SudokuSolver.txt");

        var project = ProjectParser.GetProject("SudokuSolver.csproj", projectFileText);

        Assert.Equal(Project.DotNetVersion.Framework_4_5, project.Version);
    }

    [Fact]
    public void Test_GetProject_DocVaultDAL()
    {
        var projectFileText =
            File.ReadAllLines("./TestFiles/ProjectFiles/DocVaultDAL.txt");

        var project = ProjectParser.GetProject("DocVaultDAL.csproj", projectFileText);

        Assert.Equal(Project.DotNetVersion.Net_5, project.Version);
        Assert.Equal(4, project.Packages.Count);
    }

    [Fact]
    public void Test_GetProject_GenerativeArt()
    {
        var projectFileText =
            File.ReadAllLines("./TestFiles/ProjectFiles/MicroSiteMakerServices.txt");

        var project = ProjectParser.GetProject("MicroSiteMakerServices.csproj", projectFileText);

        Assert.Equal(Project.DotNetVersion.Net_6, project.Version);
        Assert.Equal(1, project.Packages.Count);
        Assert.Equal("Markdig", project.Packages[0].Name);
        Assert.Equal("0.28.1", project.Packages[0].Version);
    }
}