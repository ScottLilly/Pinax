using Pinax.Services;

namespace Pinax.Models;

public class Job
{
    public bool IsValid => ValidationErrors.None();
    public Enums.Source LocationType { get; set; }
    public List<string> Locations { get; } =
        new List<string>();
    public List<string> ExcludedLocations { get; } =
        new List<string>();

    public List<string> ValidationErrors { get; }=
        new List<string>();
    public List<string> Results { get; } =
        new List<string>();

    public void Execute()
    {
        foreach (var location in Locations)
        {
            if (LocationType == Enums.Source.Disk)
            {
                var solutions = DiskService.GetSolutions(location);

                foreach (Solution solution in solutions)
                {
                    if (ExcludedLocations
                        .Any(x => solution.Name.StartsWith(x, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        continue;
                    }

                    Results.Add($"SOLUTION: {solution.Name}");

                    foreach (Project project in solution.Projects)
                    {
                        Results.Add($"\tPROJECT: {project.ShortName} [{string.Join(';', project.ProjectTypes)}]");

                        foreach (Package package in project.Packages)
                        {
                            Results.Add($"\t\tPACKAGE: {package.Name} {package.Version}");
                        }
                    }
                }
            }
            else
            {
                // GitHub searching
                var projects = GitHubService.GetProjectFiles(location, "C#");

                foreach (string project in projects)
                {
                    Results.Add($"PROJECT: {project}");
                }
            }
        }
    }
}