using Newtonsoft.Json;

namespace Pinax.Models.PackageManagers;

public class NuGetPackageVersions
{
    [JsonProperty("versions")]
    public string[] Versions { get; set; }
}