using Pinax.Models.Projects;

namespace Pinax.Models;

public class Solution
{
    private readonly FileDetails _fileDetails;

    public List<string> ProjectsInSolution { get; } = new();
    public List<DotNetProject> Projects { get; } = new();

    public string Path => _fileDetails.Path;
    public string Name => _fileDetails.Name;

    public string FullName =>
        System.IO.Path.Combine(Path, Name);
    public bool HasAnOutdatedProject(Enums.WarningLevel warningLevel) =>
        Projects.Any(p => p.IsOutdated(warningLevel));

    public Solution(FileDetails fileDetails)
    {
        _fileDetails = fileDetails;
    }
}