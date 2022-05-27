using Pinax.Models.Projects;

namespace Pinax.Models;

public class Solution
{
    public string Name { get; set; }

    public List<DotNetProject> Projects { get; } =
        new List<DotNetProject>();
}