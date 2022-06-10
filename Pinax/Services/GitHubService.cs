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

        IFileReader gitFileReader =
            FileReaderFactory.GetGitFileReader(s_token, username);

        var solutions = gitFileReader.GetSolutions();

        // Get project files and populate in correct solution
        var projectFiles =
            s_githubClient.Search.SearchCode(new SearchCodeRequest($"user:{username} extension:csproj")).Result;

        foreach (SearchCode projectFile in projectFiles.Items)
        {
            var project =
                new DotNetProject(projectFile.ToFileDetails());

            Solution? parentSolution =
                solutions.FirstOrDefault(s => s.Path == project.Path);

            parentSolution?.Projects.Add(project);
        }

        return solutions.ToList();
    }
}