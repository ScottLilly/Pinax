using Newtonsoft.Json;

namespace Pinax.Models.PackageManagers;

public class NuGetServices
{
    [JsonProperty("version")]
    public string Version { get; set; }
    [JsonProperty("resources")]
    public Resource[] Resources { get; set; }

    public class Resource
    {
        [JsonProperty("@id")]
        public string Id { get; set; }
        [JsonProperty("@type")]
        public string Type { get; set; }
        [JsonProperty("comment")]
        public string Comment { get; set; }
    }
}