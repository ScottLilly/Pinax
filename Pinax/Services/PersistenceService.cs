using Newtonsoft.Json;
using Pinax.Models;

namespace Pinax.Services;

public static class PersistenceService
{
    private const string LATEST_DOT_NET_VERSIONS_FILE_NAME = "LatestDotNetVersions.json";

    public static DotNetVersions GetLatestDotNetVersions()
    {
        return File.Exists(LATEST_DOT_NET_VERSIONS_FILE_NAME)
            ? JsonConvert.DeserializeObject<DotNetVersions>(File.ReadAllText(LATEST_DOT_NET_VERSIONS_FILE_NAME))
            : new DotNetVersions();
    }

    public static void OutputResults(string outputFileName, Job job)
    {
        File.WriteAllText(outputFileName, 
            JsonConvert.SerializeObject(job, Formatting.Indented));
    }
}