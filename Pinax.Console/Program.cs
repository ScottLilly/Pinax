using Microsoft.Extensions.Configuration;
using Pinax.Models;
using Pinax.Services;

Console.WriteLine("Pinax - GitHub language and library version monitoring tool");
Console.WriteLine("Type '!help' to see available commands");

Librarian librarian = SetupPinaxInstance();
DotNetVersions latestDotNetVersions =
    PersistenceService.GetLatestDotNetVersions();

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
        Console.WriteLine("--exit    Stops running Pinax");
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

} while (!command?.Equals("!exit", StringComparison.InvariantCultureIgnoreCase) ?? true);

Librarian SetupPinaxInstance()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddUserSecrets<Program>();

    var configuration = builder.Build();

    string userSecretsToken =
        configuration.AsEnumerable().First(c => c.Key == "GitHubToken").Value;

    PinaxConfiguration pinaxConfiguration =
        PersistenceService.GetPinaxConfiguration();

    GitHubService.SetToken(userSecretsToken);

    return new Librarian(pinaxConfiguration, userSecretsToken);
}