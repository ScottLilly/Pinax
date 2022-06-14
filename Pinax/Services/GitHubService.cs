using Octokit;
using Pinax.Models;
using Pinax.Models.Projects;
using Pinax.Services.FileReader;

namespace Pinax.Services;

public static class GitHubService
{
    private static string s_token = "";

    private static readonly GitHubClient s_githubClient =
        new(new ProductHeaderValue("Pinax"));

    public static void SetToken(string token)
    {
        s_token = token;
    }

    public static List<Solution> GetSolutions(string username)
    {
        var authToken = new Credentials(s_token);
        s_githubClient.Credentials = authToken;

        IFileReader gitFileReader =
            FileReaderFactory.GetGitFileReader(s_token, username);

        var solutions = gitFileReader.GetSolutions();
        var projects = gitFileReader.GetDotNetProjects();

        // Get list of projects in the .sln file
        solutions.ForEach(PopulateProjectsInSolutionFile);

        // Add projects to their solutions
        foreach (DotNetProject project in projects)
        {
            // TODO: Populate Solution.ProjectsListedInSolutionFile (from .sln file)
            // and apply "--ignoreunused" parameter, is passed
            // See existing implementation in DiskService.GetSolutions()
            Solution? parentSolution =
                solutions.FirstOrDefault(s => s.Path == project.Path);

            parentSolution?.ProjectsFound.Add(project);
        }

        return solutions.ToList();
    }

    private static void PopulateProjectsInSolutionFile(Solution solution)
    {
        // TODO: Convert to read .sln files from GitHub
        // and file the project files in them

        //var lines = File.ReadAllLines(solution.FullName);

        //foreach (string line in lines.Where(l => l.StartsWith("Project")))
        //{
        //    var projectFileName =
        //        line.Split(',')[1]
        //            .Replace("\"", "").SplitPath().Last();

        //    if (!string.IsNullOrWhiteSpace(projectFileName))
        //    {
        //        solution.ProjectsListedInSolutionFile.Add(projectFileName);
        //    }
        //}
    }
}