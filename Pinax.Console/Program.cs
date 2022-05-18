using Microsoft.Extensions.Configuration;
using Pinax.Models;
using Pinax.Services;

Console.WriteLine("Pinax - GitHub language and library version monitoring tool");
Console.WriteLine("Type '!help' to see available commands");

Librarian librarian = SetupPinaxInstance();

// Wait for user commands
string? command = "";

do
{
    command = Console.ReadLine();

    if (command == null)
    {
        continue;
    }

    if (command.Equals("!help"))
    {
        Console.WriteLine("!exit    Stops running Pinax");
    }
    else
    {
        var commandWords = command.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);

        switch (commandWords[0].ToLowerInvariant())
        {
            case "projects":
                var projects = GitHubService.GetProjectFiles("scottlilly", "C#");
                foreach (string project in projects)
                {
                    Console.WriteLine(project);
                }
                break;
            case "disk":
                if (commandWords.Length == 1)
                {
                    Console.WriteLine("'disk' command needs a path");
                    continue;
                }

                var location = string.Join(' ', commandWords.Skip(1));
                var solutions = DiskService.GetSolutions(location);
                foreach (var solution in solutions)
                {
                    Console.WriteLine($"SOLUTION: {solution.Name}");
                    foreach (var project in solution.Projects)
                    {
                        Console.WriteLine($"PROJECT: {project.FileName} VERSION: {project.Version}");
                    }
                }
                break;
            default:
                Console.WriteLine($"Unrecognized command: '{command}'");
                break;
        }
    }

} while (!command.Equals("!exit", StringComparison.InvariantCultureIgnoreCase));

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