using System.IO;
using System.Linq;
using Pinax.Models;
using Pinax.Services;
using Xunit;

namespace Test.Pinax.Services;

public class TestDiskService
{
    //[Fact]
    public void Test_DiskSearch()
    {
        var solutions =
            DiskService.GetSolutions(@"e:\MyPublicProjects\ActivityTracker");

        Assert.Equal(1, solutions.Count);
        Assert.Equal(5, solutions.Sum(s => s.Projects.Count));
    }

    //[Fact]
    public void Test_DiskSearchTopLevel()
    {
        var solutions =
            DiskService.GetSolutions(@"e:\MyPublicProjects");

        Assert.Equal(20, solutions.Count);
        Assert.Equal(70, solutions.Sum(s => s.Projects.Count));
    }
}