using Pinax.Models;
using Pinax.Services;


DisplayAppInfo();

// Wait for user commands
if (ExecuteParamCommands(args)) { return; };
do
{
    string? command = Console.ReadLine();
    RunCommands(command);
} while (true);



bool ExecuteParamCommands(string[] args)
{
    try
    {
        string param = string.Join(" ", ParamHandler(args));
        if (!string.IsNullOrEmpty(param))
        {
            RunCommands(param);
            return true;
        }
        return false;
    }
    catch { return false; }
}


/// <summary>
/// Arguments handler for parameter usage.
/// </summary>
/// <param name="args"></param>
/// <returns></returns>
List<string> ParamHandler(string[] args)
{
    List<string> argList = new List<string>();
    foreach (var arg in args)
    {
        argList.Add(arg);
    }
    return argList;
}

void RunCommands(string command)
{

    DotNetVersions latestDotNetVersions =
        PersistenceService.GetLatestDotNetVersions();
    try
    {

        if (command == null)
        {
            return;
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

        else if (command == "--exit")
        {
            Environment.Exit(0);
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
