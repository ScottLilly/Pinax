using Pinax.Models.ProjectTypes;

namespace Pinax.Models.Projects;

public class DotNetProject : IProject<DotNetProjectType>
{
    private readonly string _fileName;

    public string ShortName =>
        string.Join('\\', _fileName.Split('\\').Skip(2));

    public List<DotNetProjectType> ProjectTypes { get; } = new();
    public List<Package> Packages { get; } = new();

    public DotNetProject(string fileName)
    {
        _fileName = fileName;
    }
}