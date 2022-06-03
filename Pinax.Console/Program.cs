using Microsoft.Extensions.Configuration;
using Pinax.Models;
using Pinax.Services;

DisplayAppInfo();

DotNetVersions latestDotNetVersions =
    PersistenceService.GetLatestDotNetVersions();

SetupPinaxInstance();

// Wait for user commands
string? command = "";

do
{
    command = Console.ReadLine();

    if (command == null)
    {
        continue;
    }

    if (command.Equals("--help"))
    {
        Console.WriteLine("REQUIRED PARAMETERS");
        Console.WriteLine("--source\tValid options: disk|github");
        Console.WriteLine("--location\tGitHub URL or disk path (can be passed multiple times)");
        Console.WriteLine("");
        Console.WriteLine("OPTIONAL PARAMETERS");
        Console.WriteLine("--exclude\tGitHub URL or disk path (can be passed multiple times)");
        Console.WriteLine("--warning\tLevel to consider outdated. Valid options: major|minor|build|revision");
        Console.WriteLine("--outdated\tIf passed, only show solutions with outdated projects or packages");
        Console.WriteLine("");
        Console.WriteLine("--cls\t\tClear screen");
        Console.WriteLine("--exit\t\tStops running Pinax");
    }
    else if (command == "--cls")
    {
        Console.Clear();
        DisplayAppInfo();
    }
    else
    {
        var job =
            JobService.BuildJobFromCommand(command, latestDotNetVersions);

        if (job.IsValid)
        {
            job.Execute();

            foreach (var result in job.Results)
            {
                Console.WriteLine(result);
            }
        }
    }

} while (!command?.Equals("--exit", StringComparison.InvariantCultureIgnoreCase) ?? true);

void SetupPinaxInstance()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddUserSecrets<Program>();

    var configuration = builder.Build();

    string userSecretsToken =
        configuration.AsEnumerable().First(c => c.Key == "GitHubToken").Value;

    GitHubService.SetToken(userSecretsToken);
}

void DisplayAppInfo()
{
    Console.WriteLine("Pinax - GitHub language and library version monitoring tool");
    Console.WriteLine("Type '--help' to see available commands");
}