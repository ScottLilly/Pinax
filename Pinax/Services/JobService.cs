using Pinax.Models;

namespace Pinax.Services;

public static class JobService
{
    public static Job BuildJobFromCommand(string command,
        DotNetVersions dotNetVersions)
    {
        var commands =
            command.Trim().Split("--", StringSplitOptions.RemoveEmptyEntries);

        Enums.Source source = Enums.Source.Unknown;
        List<string> includedLocations = new List<string>();
        List<string> excludedLocations = new List<string>();
        Enums.WarningLevel warningLevel = Enums.WarningLevel.Minor;
        bool showOutdatedSolutionsOnly = false;

        foreach (var cmd in commands)
        {
            string key = "";
            string val = "";

            if (cmd.Contains(":"))
            {
                key = cmd[..cmd.IndexOf(":")];
                val = cmd[(cmd.IndexOf(":") + 1)..];
            }
            else
            {
                key = cmd;
            }

            //if (string.IsNullOrWhiteSpace(key) ||
            //    string.IsNullOrWhiteSpace(val))
            //{
            //    continue;
            //}

            // Handle key/value parameters
            if (key.Matches("source"))
            {
                source = Enum.Parse<Enums.Source>(val, true);
            }
            else if (key.Matches("location"))
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
                showOutdatedSolutionsOnly = true;
            }
        }

        // TODO: Handle bad parameters

        var job =
            new Job(source, dotNetVersions, warningLevel, showOutdatedSolutionsOnly);

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