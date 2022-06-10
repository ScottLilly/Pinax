using Pinax.Models;
using Pinax.Models.Projects;
using Pinax.Services.FileReader;

namespace Pinax.Services;

public static class DiskService
{
    public static List<Solution> GetSolutions(string rootDirectory,
        DotNetVersions latestVersions, bool ignoreUnusedProjects)
    {
        if (!Directory.Exists(rootDirectory))
        {
            Console.WriteLine($"Directory '{rootDirectory}' does not exist");
        }

        var diskFileReader = FileReaderFactory.GetDiskFileReader(rootDirectory);

        var solutionFiles = diskFileReader.GetSolutionFiles();

        var solutions =
            solutionFiles.Select(s => new Solution(s)).ToList();

        foreach (Solution solution in solutions)
        {
            PopulateProjectsInSolutionFile(solution);

            var projectFiles =
                Directory.GetFiles(solution.Path,
                    "*.csproj", SearchOption.AllDirectories);

            if (ignoreUnusedProjects)
            {
                projectFiles =
                    projectFiles.Where(p =>
                        solution.ProjectsInSolution.Contains(p.SplitPath().Last())).ToArray();
            }

            foreach (var projectFile in projectFiles)
            {
                DotNetProject project = 
                    ProjectParser.GetProject(projectFile, 
                        File.ReadAllLines(projectFile), latestVersions);

                solution.Projects.Add(project);
            }
        }

        return solutions.ToList();
    }

    private static void PopulateProjectsInSolutionFile(Solution solution)
    {
        var lines = File.ReadAllLines(solution.FullName);

        foreach (string line in lines.Where(l => l.StartsWith("Project")))
        {
            var projectFileName =
                line.Split(',')[1]
                    .Replace("\"", "").SplitPath().Last();

            if (!string.IsNullOrWhiteSpace(projectFileName))
            {
                solution.ProjectsInSolution.Add(projectFileName);
            }
        }
    }
}