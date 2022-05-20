using Pinax.Models;

namespace Pinax.Services;

public static class JobService
{
    public static Job BuildJobFromCommand(string command)
    {
        var job = new Job();

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
            }
        }

        return job;
    }
}