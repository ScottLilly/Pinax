using Pinax.Engine.Models;

namespace Pinax.Engine.Services;

public static class JobService
{
    public static Job BuildJobFromCommand(string command,
        DotNetVersions dotNetVersions)
    {
        var commands =
            command.Trim().Split("--", StringSplitOptions.RemoveEmptyEntries);

        Enums.Source source = Enums.Source.Disk;
        List<string> includedLocations = new List<string>();
        List<string> excludedLocations = new List<string>();
        Enums.WarningLevel warningLevel = Enums.WarningLevel.Minor;
        bool onlyShowOutdated = false;
        bool ignoreUnusedProjects = false;
        string outputFileName = "";

        foreach (var cmd in commands)
        {
            string key = "";
            string val = "";

            if (cmd.Contains(":"))
            {
                key = cmd[..cmd.IndexOf(":")].Trim();
                val = cmd[(cmd.IndexOf(":") + 1)..].Trim();
            }
            else
            {
                key = cmd.Trim();
            }

            // Handle key/value parameters
            if (key.Matches("location"))
            {
                includedLocations.Add(val.Trim());
            }
            else if (key.Matches("exclude"))
            {
                excludedLocations.Add(val.Trim());
            }
            else if (key.Matches("warning"))
            {
                warningLevel = Enum.Parse<Enums.WarningLevel>(val, true);
            }
            else if (key.Matches("outdated"))
            {
                onlyShowOutdated = true;
            }
            else if (key.Matches("ignoreunused"))
            {
                ignoreUnusedProjects = true;
            }
            else if (key.Matches("output"))
            {
                outputFileName = val.Trim();
            }
        }

        // TODO: Handle bad parameters

        var job =
            new Job(dotNetVersions, warningLevel, 
                onlyShowOutdated, ignoreUnusedProjects, outputFileName);

        foreach (string location in includedLocations)
        {
            job.AddIncludedLocation(location);
        }

        foreach (string location in excludedLocations)
        {
            job.AddExcludedLocation(location);
        }

        return job;
    }
}