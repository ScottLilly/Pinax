using Pinax.Models.Projects;

namespace Pinax.Models;

public class Solution
{
    private readonly FileDetails _fileDetails;

    public List<string> ProjectsListedInSolutionFile { get; } = new();
    public List<DotNetProject> ProjectsFound { get; } = new();

    public string Path => _fileDetails.Path;
    public string Name => _fileDetails.Name;

    public string FullName =>
        System.IO.Path.Combine(Path, Name);
    public bool HasAnOutdatedProject(DotNetVersions dotNetVersions,
        Enums.WarningLevel warningLevel) =>
        ProjectsFound.Any(p => p.IsOutdated(dotNetVersions, warningLevel));

    public Solution(FileDetails fileDetails)
    {
        _fileDetails = fileDetails;
    }
}