using Newtonsoft.Json;
using Pinax.Models;

namespace Pinax.Services;

public static class PersistenceService
{
    private const string PINAX_CONFIGURATION_FILE_NAME = "appsettings.json";

    public static PinaxConfiguration GetPinaxConfiguration()
    {
        PinaxConfiguration wobbleConfiguration =
            File.Exists(PINAX_CONFIGURATION_FILE_NAME)
                ? JsonConvert.DeserializeObject<PinaxConfiguration>(File.ReadAllText(PINAX_CONFIGURATION_FILE_NAME))
                : new PinaxConfiguration();

        return wobbleConfiguration;
    }
}