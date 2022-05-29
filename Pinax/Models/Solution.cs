using Pinax.Models.Projects;

namespace Pinax.Models;

public class Solution
{
    public string Name { get; set; }

    public bool HasOutdatedSolutions(Enums.WarningLevel warningLevel) =>
        Projects.Any(p => p.IsOutdated(warningLevel));

    public List<DotNetProject> Projects { get; } =
        new List<DotNetProject>();
}