using Pinax.Models.Projects;
using Pinax.Services;

namespace Pinax.Models;

public class Job
{
    private readonly Enums.Source _fileSource;
    private readonly DotNetVersions _latestDotNetVersions;
    private readonly Enums.WarningLevel _warningLevel;
    private readonly List<string> _includedLocations = new();
    private readonly List<string> _excludedLocations = new();

    public List<Solution> Solutions =
        new List<Solution>();
    public List<string> Results { get; } =
        new List<string>();
    public List<string> ValidationErrors { get; } =
        new List<string>();

    public bool IsValid => ValidationErrors.None();

    public Job(Enums.Source fileSource,
        DotNetVersions latestDotNetVersions,
        Enums.WarningLevel warningLevel = Enums.WarningLevel.None)
    {
        _fileSource = fileSource;
        _latestDotNetVersions = latestDotNetVersions;
        _warningLevel = warningLevel;
    }

    public void AddIncludedLocation(string location)
    {
        if (_includedLocations.None(l => l.Matches(location)))
        {
            _includedLocations.Add(location);
        }
    }

    public void AddExcludedLocation(string location)
    {
        if (_excludedLocations.None(l => l.Matches(location)))
        {
            _excludedLocations.Add(location);
        }
    }

    public void Execute()
    {
        PopulateSolutions();

        foreach (Solution solution in Solutions)
        {
            Results.Add($"SOLUTION: {solution.Name}");

            foreach (DotNetProject project in solution.Projects)
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

    #region Private methods

    private void PopulateSolutions()
    {
        foreach (var location in _includedLocations)
        {
            switch (_fileSource)
            {
                case Enums.Source.Disk:
                    PopulateSolutionsFromDisk(location);
                    break;
                case Enums.Source.GitHub:
                    PopulateSolutionsFromGitHub(location);
                    break;
            }
        }
    }

    private void PopulateSolutionsFromDisk(string location)
    {
        Solutions =
            DiskService.GetSolutions(location)
                .Where(s =>
                    _excludedLocations.None(e =>
                        s.Name.StartsWith(e, StringComparison.InvariantCultureIgnoreCase)))
                .ToList();
    }

    private void PopulateSolutionsFromGitHub(string location)
    {
        // TODO: Read solutions from GitHub
        // GitHub searching
        var projects = GitHubService.GetProjectFiles(location, "C#");

        foreach (string project in projects)
        {
            Results.Add($"PROJECT: {project}");
        }
    }

    #endregion
}