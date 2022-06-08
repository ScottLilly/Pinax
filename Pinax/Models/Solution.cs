using Pinax.Models.Projects;

namespace Pinax.Models;

public class Solution
{
    public string Path { get; }
    public string Name { get; }

    public List<string> ProjectsInSolution { get; } = new();
    public List<DotNetProject> Projects { get; } = new();

    public bool HasAnOutdatedProject(Enums.WarningLevel warningLevel) =>
        Projects.Any(p => p.IsOutdated(warningLevel));

    public Solution(string path, string name)
    {
        Path = path;
        Name = name;
    }
}