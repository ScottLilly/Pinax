namespace Pinax.Models;

public class Project
{
    public string FileName { get; set; }
    public string ShortName =>
        string.Join('\\', FileName.Split('\\').Skip(2));

    public List<ProjectType> ProjectTypes { get; set; } =
        new List<ProjectType>();
    public List<Package> Packages { get; } =
        new List<Package>();
}