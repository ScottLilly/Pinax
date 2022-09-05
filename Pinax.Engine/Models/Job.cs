using Newtonsoft.Json;
using Pinax.Engine.Models.Projects;
using Pinax.Engine.Services;

namespace Pinax.Engine.Models;

public class Job
{
    private readonly Enums.Source _fileSource;
    private readonly DotNetVersions _latestDotNetVersions;
    private readonly Enums.WarningLevel _warningLevel;
    private readonly bool _onlyShowOutdated;
    private readonly bool _ignoreUnusedProjects;
    private readonly List<string> _includedLocations = new();
    private readonly List<string> _excludedLocations = new();

    private readonly List<Solution> _solutions = new();

    [JsonProperty("Solutions")]
    public IEnumerable<Solution> SolutionsToDisplay =>
        _onlyShowOutdated
            ? _solutions.Where(s => s.HasAnOutdatedProject(_latestDotNetVersions, _warningLevel))
            : _solutions;

    [Newtonsoft.Json.JsonIgnore]
    public List<string> Results { get; } = new();
    [Newtonsoft.Json.JsonIgnore]
    public List<string> ValidationErrors { get; } = new();
    [Newtonsoft.Json.JsonIgnore]
    public string OutputFileName { get; } = "";

    [Newtonsoft.Json.JsonIgnore]
    public bool IsValid => ValidationErrors.None();

    public Job(Enums.Source fileSource,
        DotNetVersions latestDotNetVersions,
        Enums.WarningLevel warningLevel,
        bool onlyShowOutdated,
        bool ignoreUnusedProjects, 
        string outputFileName)
    {
        _fileSource = fileSource;
        _latestDotNetVersions = latestDotNetVersions;
        _warningLevel = warningLevel;
        _onlyShowOutdated = onlyShowOutdated;
        _ignoreUnusedProjects = ignoreUnusedProjects;

        OutputFileName = outputFileName;
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
            Results.Add($"SOLUTION: {Path.Combine(solution.Path, solution.Name)}");

            foreach (DotNetProject project in solution.ProjectsFound)
            {
                bool isOutdated = project.IsOutdated(_latestDotNetVersions, _warningLevel);

                string outdatedProjectIndicator = isOutdated ? "*" : "";
                bool notInSolution =
                    solution.ProjectsListedInSolutionFile.None(p => p == project.Name);
                string notInSolutionIndicator = notInSolution ? "?" : "";

                Results.Add($"{notInSolutionIndicator}{outdatedProjectIndicator}\tPROJECT: {project.Name} [{string.Join(';', project.ProjectTypes)}]");

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
            DiskService.GetSolutions(location, _ignoreUnusedProjects)
                .Where(s =>
                    _excludedLocations.None(e =>
                        s.Name.StartsWith(e, StringComparison.InvariantCultureIgnoreCase)))
                .ToList());
    }

    private void PopulateSolutionsFromGitHub(string location)
    {
        var solutions =
            GitHubService.GetSolutions(location);

        _solutions.AddRange(solutions);
    }

    #endregion
}