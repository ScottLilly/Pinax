using Octokit;
using Pinax.Models;

namespace Pinax.Services.FileReader;

public class GitFileReader : IFileReader
{
    private readonly GitHubClient _githubClient =
        new(new ProductHeaderValue("Pinax"));

    private readonly string _path;

    public GitFileReader(string token, string path)
    {
        _githubClient.Credentials = new Credentials(token);
        _path = path;
    }

    public IEnumerable<FileDetails> GetSolutionFiles()
    {
        return _githubClient
            .Search
            .SearchCode(new SearchCodeRequest($"user:{_path} extension:sln"))
            .Result
            .Items
            .Select(i => new FileDetails(i.Repository.HtmlUrl, i.Name));
    }
}