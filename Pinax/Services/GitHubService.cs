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

    public static List<Solution> GetSolutions(string username,
        DotNetVersions latestVersions)
    {
        var authToken = new Credentials(s_token);
        s_githubClient.Credentials = authToken;

        var gitFileReader = FileReaderFactory.GetGitFileReader(s_token, username);

        var solutionFiles = gitFileReader.GetSolutionFiles();

        var solutions =
            solutionFiles.Select(s => new Solution(s)).ToList();

        // Get project files and populate in correct solution
        var projectFiles =
            s_githubClient.Search.SearchCode(new SearchCodeRequest($"user:{username} extension:csproj")).Result;

        foreach (SearchCode projectFile in projectFiles.Items)
        {
            var project =
                new DotNetProject(projectFile.Repository.HtmlUrl, 
                    projectFile.Name, latestVersions);

            Solution? parentSolution =
                solutions.FirstOrDefault(s => s.Path == project.Path);

            parentSolution?.Projects.Add(project);
        }

        return solutions.ToList();
    }
}