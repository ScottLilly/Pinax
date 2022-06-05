using Pinax.Models.Projects;

namespace Pinax.Models;

public class Solution
{
    public string Name { get; set; }

    public bool HasAnOutdatedProject(Enums.WarningLevel warningLevel) =>
        Projects.Any(p => p.IsOutdated(warningLevel));

    public List<string> ProjectsInSolution { get; } = new();
    public List<DotNetProject> Projects { get; } = new();
}