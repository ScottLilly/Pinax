using Pinax.Engine.Models;
using Pinax.Engine.Models.Projects;

namespace Pinax.Engine.Services.FileReader;

public class DiskFileReader : IFileReader
{
    private readonly string _path;

    public DiskFileReader(string path)
    {
        _path = path;
    }

    public List<Solution> GetSolutions()
    {
        return 
            (Directory.GetFiles(_path, "*.sln", SearchOption.AllDirectories)
                .Select(f => new FileInfo(f))
                .Select(fi => new FileDetails(fi.DirectoryName ?? _path, fi.Name)))
            .Select(fd => new Solution(fd))
            .ToList();
    }

    public List<DotNetProject> GetDotNetProjects()
    {
        return
            (Directory.GetFiles(_path, "*.csproj", SearchOption.AllDirectories)
                .Select(f => new FileInfo(f))
                .Select(fi => new FileDetails(fi.DirectoryName ?? _path, fi.Name)))
            .Select(fd => new DotNetProject(fd))
            .ToList();
    }
}