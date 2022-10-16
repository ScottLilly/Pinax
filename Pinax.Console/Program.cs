using Pinax.Engine;
using Pinax.Engine.Models;
using Pinax.Engine.Services;

DisplayAppInfo();

DotNetVersions latestDotNetVersions =
    PersistenceService.GetLatestDotNetVersions();

// If parameters were passed in, run the job for them
if (args.Any())
{
    RunCommand(string.Join(' ', args));
};

// Wait for user commands
while(true)
{
    string? command = Console.ReadLine();

    if (command == null || string.IsNullOrWhiteSpace(command))
    {
        continue;
    }
    
    if (command.Matches("--exit"))
    {
        break;
    }

    if (command.Matches("--help"))
    {
        DisplayHelpText();
    }
    else if (command.Matches("--cls"))
    {
        Console.Clear();
        DisplayAppInfo();
    }
    else
    {
        RunCommand(command);
    }
}

void RunCommand(string command)
{
    if (string.IsNullOrWhiteSpace(command))
    {
        return;
    }

    try
    {
        var job =
            JobService.BuildJobFromCommand(command, latestDotNetVersions);

        if (!job.IsValid)
        {
            WriteLine("Invalid parameters. Unable to run the job.", ConsoleColor.Red);
            return;
        }

        WriteLine("Version check started", ConsoleColor.Green);

        job.Execute();
            
        // Display results
        foreach (var result in job.Results)
        {
            WriteLine(result);
        }

        if (job.OutputFileName.IsNotNullEmptyOrWhiteSpace())
        {
            PersistenceService.OutputResults(job.OutputFileName, job);

            WriteLine($"Created file: {job.OutputFileName}", ConsoleColor.Cyan);
        }

        WriteLine("Version check completed", ConsoleColor.Green);
    }
    catch (Exception e)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine(e);
        Console.ForegroundColor = ConsoleColor.White;
    }
}

void DisplayAppInfo()
{
    Console.WriteLine("Pinax - GitHub language and library version monitoring tool");
    Console.WriteLine("Type '--help' to see available commands");
}

void DisplayHelpText()
{
    Console.WriteLine("REQUIRED PARAMETERS");
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

void WriteLine(string message, ConsoleColor color = ConsoleColor.White)
{
    Console.ForegroundColor = color;
    Console.WriteLine(message);
    Console.ForegroundColor = ConsoleColor.White;
}