using Newtonsoft.Json;
using Pinax.Models;

namespace Pinax.Services;

public static class PersistenceService
{
    private const string PINAX_CONFIGURATION_FILE_NAME = "appsettings.json";
    private const string LATEST_DOT_NET_VERSIONS_FILE_NAME = "DotNetVersions.json";

    public static PinaxConfiguration GetPinaxConfiguration()
    {
        return File.Exists(PINAX_CONFIGURATION_FILE_NAME)
            ? JsonConvert.DeserializeObject<PinaxConfiguration>(File.ReadAllText(PINAX_CONFIGURATION_FILE_NAME))
            : new PinaxConfiguration();
    }

    public static DotNetVersions GetLatestDotNetVersions()
    {
        return File.Exists(LATEST_DOT_NET_VERSIONS_FILE_NAME)
            ? JsonConvert.DeserializeObject<DotNetVersions>(File.ReadAllText(LATEST_DOT_NET_VERSIONS_FILE_NAME))
            : new DotNetVersions();
    }
}