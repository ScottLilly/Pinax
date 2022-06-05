using Pinax.Models;
using Pinax.Models.Projects;

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

        var solutions = new List<Solution>();

        var solutionFiles =
            Directory.GetFiles(rootDirectory, "*.sln", SearchOption.AllDirectories);

        foreach (string solutionFile in solutionFiles)
        {
            var solutionFileInfo = new FileInfo(solutionFile);

            var solution = new Solution
            {
                Name = solutionFile
            };

            solution.ProjectsInSolution
                .AddRange(GetProjectsInSolution(solutionFileInfo));

            var projectFiles =
                Directory.GetFiles(solutionFileInfo.DirectoryName,
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

            solutions.Add(solution);
        }

        return solutions;
    }

    private static List<string> GetProjectsInSolution(FileInfo solutionFileInfo)
    {
        List<string> projectsInSolution = new();

        var lines = File.ReadAllLines(solutionFileInfo.FullName);

        foreach (string line in lines.Where(l => l.StartsWith("Project")))
        {
            var projectFileName =
                line.Split(',')[1]
                    .Replace("\"", "").SplitPath().Last();

            if (!string.IsNullOrWhiteSpace(projectFileName))
            {
                projectsInSolution.Add(projectFileName);
            }
        }

        return projectsInSolution;
    }
}