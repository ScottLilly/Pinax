namespace Pinax.Models;

public class Solution
{
    public string Name { get; set; }

    public List<Project> Projects { get; } =
        new List<Project>();
}