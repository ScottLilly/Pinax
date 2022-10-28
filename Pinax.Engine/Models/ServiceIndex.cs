using Newtonsoft.Json;

namespace Pinax.Engine.Models;

public class ServiceIndex
{
    public string version { get; set; }
    public Resource[] resources { get; set; }

    [JsonIgnore]
    public int ProtocolVersion =>
        Convert.ToInt32(version.Split('.')[0] ?? "0");

    public class Resource
    {
        public string id { get; set; }
        public string type { get; set; }
        public string comment { get; set; }
    }
}
