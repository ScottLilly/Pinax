using Pinax.Services;

namespace Pinax.Models;

public class Job
{
    public bool IsValid => ValidationErrors.None();
    public Enums.Source LocationType { get; set; }
    public List<string> Locations { get; } =
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

                foreach (var solution in solutions)
                {
                    Results.Add($"SOLUTION: {solution.Name}");

                    foreach (var project in solution.Projects)
                    {
                        Results.Add($"PROJECT: {project.FileName} VERSION: {project.Version}");
                    }
                }
            }
            else
            {
                // GitHub searching
            }
        }
    }
}