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
    try
    {
        command = Console.ReadLine();

        if (command == null)
        {
            continue;
        }

        if (command.Equals("--help"))
        {
            DisplayHelpText();
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

                if (string.IsNullOrEmpty(job.OutputFileName))
                {
                    foreach (var result in job.Results)
                    {
                        Console.WriteLine(result);
                    }
                }
                else
                {
                    PersistenceService.OutputResults(job.OutputFileName, job);
                    Console.WriteLine($"Created file: {job.OutputFileName}");
                }
            }
        }
    }
    catch (Exception e)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine(e);
        Console.ForegroundColor = ConsoleColor.White;
        command = "--exit";
    }

} while (!command?.Equals("--exit", StringComparison.InvariantCultureIgnoreCase) ?? true);

void SetupPinaxInstance()
{
    // Currently not connecting to GitHub, due to throttling issues

    //var builder = new ConfigurationBuilder()
    //    .SetBasePath(Directory.GetCurrentDirectory())
    //    .AddUserSecrets<Program>();

    //var configuration = builder.Build();

    //string userSecretsToken =
    //    configuration.AsEnumerable().First(c => c.Key == "GitHubToken").Value;

    //GitHubService.SetToken(userSecretsToken);
}

void DisplayAppInfo()
{
    Console.WriteLine("Pinax - GitHub language and library version monitoring tool");
    Console.WriteLine("Type '--help' to see available commands");
}

void DisplayHelpText()
{
    Console.WriteLine("REQUIRED PARAMETERS");
    Console.WriteLine("--source\tValid options: disk|github");
    Console.WriteLine("--location\tDisk path (can be passed multiple times)");
    Console.WriteLine("");
    Console.WriteLine("OPTIONAL PARAMETERS");
    Console.WriteLine("--exclude\tDisk path (can be passed multiple times)");
    Console.WriteLine("--warning\tVersion level to consider outdated. Valid options: major|minor|build|revision");
    Console.WriteLine("--outdated\tIf passed, only show solutions with outdated projects or packages");
    Console.WriteLine("--ignoreunused\tIf passed, does not check projects that are not in the solution file");
    Console.WriteLine("");
    Console.WriteLine("--cls\t\tClear screen");
    Console.WriteLine("--exit\t\tStops running Pinax");
}
