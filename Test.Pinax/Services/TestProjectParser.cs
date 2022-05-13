using System.IO;
using Pinax.Services;
using Xunit;

namespace Test.Pinax.Services;

public class TestProjectParser
{
    [Fact]
    public void Test_GetVersionNumber_Framework_4_5()
    {
        string projectFileText =
            File.ReadAllText("./TestFiles/ProjectFiles/SudokuSolver.txt");

        var version = ProjectParser.GetVersion(projectFileText);

        Assert.Equal(ProjectParser.DotNetVersion.Framework_4_5, version);
    }

    [Fact]
    public void Test_GetVersionNumber_NET_6()
    {
        string projectFileText =
            File.ReadAllText("./TestFiles/ProjectFiles/GenerativeArt.txt");

        var version = ProjectParser.GetVersion(projectFileText);

        Assert.Equal(ProjectParser.DotNetVersion.Net_6, version);
    }
}