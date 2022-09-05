using Pinax.Models;
using Pinax.Models.Projects;
using Pinax.Services.FileReader;

namespace Pinax.Services;

public static class DiskService
{
    public static List<Solution> GetSolutions(string rootDirectory,
        bool ignoreUnusedProjects)
    {
        if (!Directory.Exists(rootDirectory))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Directory '{rootDirectory}' does not exist");
            Console.ForegroundColor = ConsoleColor.White;

            return new List<Solution>();
        }

        IFileReader diskFileReader =
            FileReaderFactory.GetDiskFileReader(rootDirectory);

        var solutions = diskFileReader.GetSolutions();
        var projects = diskFileReader.GetDotNetProjects();

        // Get list of projects in the .sln file
        solutions.ForEach(PopulateProjectsInSolutionFile);

        // Add projects to their solutions
        foreach (Solution solution in solutions)
        {
            // Get the projects underneath the solution directory
            var projectsForSolution =
                projects.Where(p => p.Path.StartsWith(solution.Path));

            // Filter out projects in directory, but not in .sln file,
            // if ignoring projects that aren't in the solution file
            if (ignoreUnusedProjects)
            {
                projectsForSolution =
                    projectsForSolution.Where(p =>
                        solution.ProjectsListedInSolutionFile.Contains(p.Name));
            }

            foreach (DotNetProject projectFile in projectsForSolution)
            {
                DotNetProject project =
                    ProjectParser.ParseProjectFileText(projectFile.FullName,
                        File.ReadAllLines(projectFile.FullName));

                solution.ProjectsFound.Add(project);
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
                solution.ProjectsListedInSolutionFile.Add(projectFileName);
            }
        }
    }
}