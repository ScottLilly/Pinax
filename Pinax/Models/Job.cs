using Pinax.Models.Projects;
using Pinax.Services;

namespace Pinax.Models;

public class Job
{
    private readonly Enums.Source _fileSource;
    private readonly DotNetVersions _latestDotNetVersions;
    private readonly Enums.WarningLevel _warningLevel;
    private readonly bool _onlyShowOutdated;
    private readonly List<string> _includedLocations = new();
    private readonly List<string> _excludedLocations = new();

    private readonly List<Solution> _solutions = new();

    private IEnumerable<Solution> SolutionsToDisplay =>
        _onlyShowOutdated
            ? _solutions.Where(s => s.HasOutdatedSolutions(_warningLevel))
            : _solutions;

    public List<string> Results { get; } = new();
    public List<string> ValidationErrors { get; } = new();

    public bool IsValid => ValidationErrors.None();

    public Job(Enums.Source fileSource,
        DotNetVersions latestDotNetVersions,
        Enums.WarningLevel warningLevel,
        bool onlyShowOutdated)
    {
        _fileSource = fileSource;
        _latestDotNetVersions = latestDotNetVersions;
        _warningLevel = warningLevel;
        _onlyShowOutdated = onlyShowOutdated;
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

        foreach (Solution solution in SolutionsToDisplay)
        {
            Results.Add($"SOLUTION: {solution.Name}");

            foreach (DotNetProject project in solution.Projects)
            {
                bool isOutdated = project.IsOutdated(_warningLevel);

                string outdatedProjectFlag = isOutdated ? "*" : "";

                Results.Add($"{outdatedProjectFlag}\tPROJECT: {project.ShortName} [{string.Join(';', project.ProjectTypes)}]");

                foreach (Package package in project.Packages)
                {
                    string outdatedPackageFlag = package.IsOutdated(_warningLevel) ? "*" : "";

                    Results.Add($"{outdatedPackageFlag}\t\tPACKAGE: {package}");
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
        _solutions.AddRange(
            DiskService.GetSolutions(location, _latestDotNetVersions)
                .Where(s =>
                    _excludedLocations.None(e =>
                        s.Name.StartsWith(e, StringComparison.InvariantCultureIgnoreCase)))
                .ToList());
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