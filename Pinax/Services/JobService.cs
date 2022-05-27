using Pinax.Models;

namespace Pinax.Services;

public static class JobService
{
    public static Job BuildJobFromCommand(string command,
        DotNetVersions dotNetVersions)
    {
        var job = new Job(dotNetVersions);

        var commands =
            command.Trim().Split("--", StringSplitOptions.RemoveEmptyEntries);

        foreach (var cmd in commands)
        {
            string key = "";
            string val = "";
            if (cmd.Contains(":"))
            {
                key = cmd[..cmd.IndexOf(":")];
                val = cmd[(cmd.IndexOf(":")+1)..];
            }

            if (!string.IsNullOrWhiteSpace(key) &&
                !string.IsNullOrWhiteSpace(val))
            {
                if (key.Matches("source"))
                {
                    job.LocationType = Enum.Parse<Enums.Source>(val, true);
                }
                else if (key.Matches("location"))
                {
                    job.Locations.Add(val.Trim());
                }
                else if (key.Matches("exclude"))
                {
                    job.ExcludedLocations.Add(val.Trim());
                }
            }
        }

        return job;
    }
}