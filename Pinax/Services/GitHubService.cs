using Octokit;

namespace Pinax.Services;

public static class GitHubService
{
    private static string s_token = "";
    private static readonly GitHubClient s_githubClient =
        new GitHubClient(new ProductHeaderValue("Pinax"));

    public static void SetToken(string token)
    {
        s_token = token;
    }

    public static void GetUserInfo(string username)
    {
        var user = s_githubClient.User.Get(username).Result;

        Console.WriteLine($"{user.Followers} folks love {username}!");
    }

    public static List<string> GetProjectFiles(string username, string language)
    {
        var authToken = new Credentials(s_token);
        s_githubClient.Credentials = authToken;

        var repositories =
            s_githubClient.Repository.GetAllForUser(username).Result.ToList()
                .Where(r => r.Language == language).ToList();

        var projects = new List<string>();

        foreach (Repository repository in repositories)
        {
            var results =
                s_githubClient.Search.SearchCode(new SearchCodeRequest($"repo:{repository.FullName} extension:csproj")).Result;

            foreach (SearchCode resultsItem in results.Items)
            {
                projects.Add(Path.Combine(repository.FullName, resultsItem.Path));
            }
        }

        return projects;
    }
}