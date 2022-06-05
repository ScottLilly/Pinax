using Octokit;
using Pinax.Models;
using Pinax.Models.Projects;

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

    public static List<Solution> GetSolutions(string username,
        DotNetVersions latestVersions, bool ignoreUnusedProjects)
    {
        var authToken = new Credentials(s_token);
        s_githubClient.Credentials = authToken;

        var solutionFiles =
            s_githubClient.Search.SearchCode(new SearchCodeRequest($"user:{username} extension:sln")).Result;

        List<Solution> solutions = new List<Solution>();

        foreach (SearchCode solutionFile in solutionFiles.Items)
        {
            solutions.Add(new Solution
            {
                Path = solutionFile.Repository.HtmlUrl,
                Name = Path.Combine(solutionFile.Name)
            });
        }

        // Get project files and populate in correct solution
        var projectFiles =
            s_githubClient.Search.SearchCode(new SearchCodeRequest($"user:{username} extension:csproj")).Result;

        foreach (SearchCode projectFile in projectFiles.Items)
        {
            var project = new DotNetProject(projectFile.Name, latestVersions);
            project.Path = projectFile.Repository.HtmlUrl;

            Solution? parentSolution =
                solutions.FirstOrDefault(s => s.Path == project.Path);

            parentSolution?.Projects.Add(project);
        }

        return solutions;
    }
}