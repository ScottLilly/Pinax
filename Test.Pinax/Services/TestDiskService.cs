using System;
using System.IO;
using System.Linq;
using Pinax.Models;
using Pinax.Services;
using Xunit;

namespace Test.Pinax.Services;

public class TestDiskService
{
    private readonly DotNetVersions _dotNetVersions =
        new()
        {
            Standard = new Version(2, 0),
            Core = new Version(3, 1),
            Framework = new Version(4, 8),
            DotNet = new Version(6, 0)
        };

    //[Fact]
    public void Test_DiskSearch()
    {
        var solutions =
            DiskService.GetSolutions(@"e:\MyPublicProjects\ActivityTracker", _dotNetVersions);

        Assert.Equal(1, solutions.Count);
        Assert.Equal(5, solutions.Sum(s => s.Projects.Count));
    }

    //[Fact]
    public void Test_DiskSearchTopLevel()
    {
        var solutions =
            DiskService.GetSolutions(@"e:\MyPublicProjects", _dotNetVersions);

        Assert.Equal(20, solutions.Count);
        Assert.Equal(70, solutions.Sum(s => s.Projects.Count));
    }
}