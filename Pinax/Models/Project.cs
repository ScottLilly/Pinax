using Pinax.Models.ProjectTypes;

namespace Pinax.Models;

public class Project
{
    public string FileName { get; }
    public string ShortName =>
        string.Join('\\', FileName.Split('\\').Skip(2));

    public List<DotNetProjectType> ProjectTypes { get; } =
        new List<DotNetProjectType>();
    public List<Package> Packages { get; } =
        new List<Package>();

    public Project(string fileName)
    {
        FileName = fileName;
    }
}