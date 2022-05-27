using Pinax.Services;

namespace Pinax.Models;

public class Job
{
    private readonly DotNetVersions _latestDotNetVersions;

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

    public Job(DotNetVersions latestDotNetVersions)
    {
        _latestDotNetVersions = latestDotNetVersions;
    }

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
                        bool isOutdated = false;

                        if (project.ProjectTypes.Any(p => p.Type == Enums.DotNetType.Standard) &&
                            project.ProjectTypes
                                .Where(p => p.Type == Enums.DotNetType.Standard)
                                .None(p => p.Version.Major == _latestDotNetVersions.Standard.Major &&
                                           p.Version.Minor == _latestDotNetVersions.Standard.Minor))
                        {
                            isOutdated = true;
                        }

                        if (project.ProjectTypes.Any(p => p.Type == Enums.DotNetType.Core) &&
                            project.ProjectTypes
                                .Where(p => p.Type == Enums.DotNetType.Core)
                                .None(p => p.Version.Major == _latestDotNetVersions.Core.Major &&
                                           p.Version.Minor == _latestDotNetVersions.Core.Minor))
                        {
                            isOutdated = true;
                        }

                        if (project.ProjectTypes.Any(p => p.Type == Enums.DotNetType.Framework) &&
                            project.ProjectTypes
                                .Where(p => p.Type == Enums.DotNetType.Framework)
                                .None(p => p.Version.Major == _latestDotNetVersions.Framework.Major &&
                                           p.Version.Minor == _latestDotNetVersions.Framework.Minor))
                        {
                            isOutdated = true;
                        }

                        if (project.ProjectTypes.Any(p => p.Type == Enums.DotNetType.DotNet) &&
                            project.ProjectTypes
                                .Where(p => p.Type == Enums.DotNetType.DotNet)
                                .None(p => p.Version.Major == _latestDotNetVersions.DotNet.Major &&
                                           p.Version.Minor == _latestDotNetVersions.DotNet.Minor))
                        {
                            isOutdated = true;
                        }

                        string outdatedFlag = isOutdated ? "*" : "";

                        Results.Add($"{outdatedFlag}\tPROJECT: {project.ShortName} [{string.Join(';', project.ProjectTypes)}]");

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