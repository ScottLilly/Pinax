using Newtonsoft.Json;

namespace Pinax.Models.PackageManagers;

public class NuGetPackageDetails
{
    [JsonProperty("@id")]
    public string id { get; set; }
    [JsonProperty("@type")]
    public string[] type { get; set; }
    public string commitId { get; set; }
    public DateTime commitTimeStamp { get; set; }
    public int count { get; set; }
    public Item[] items { get; set; }
    public Context context { get; set; }

    public class Context
    {
        public string vocab { get; set; }
        public string catalog { get; set; }
        public string xsd { get; set; }
        public Items items { get; set; }
        public Committimestamp commitTimeStamp { get; set; }
        public Commitid commitId { get; set; }
        public Count count { get; set; }
        public Parent parent { get; set; }
        public Tags tags { get; set; }
        public Reasons reasons { get; set; }
        public Packagetargetframeworks packageTargetFrameworks { get; set; }
        public Dependencygroups dependencyGroups { get; set; }
        public Dependencies dependencies { get; set; }
        public Packagecontent packageContent { get; set; }
        public Published published { get; set; }
        public Registration registration { get; set; }
    }

    public class Items
    {
        [JsonProperty("@id")]
        public string id { get; set; }
        public string container { get; set; }
    }

    public class Committimestamp
    {
        [JsonProperty("@id")]
        public string id { get; set; }
        [JsonProperty("@type")]
        public string type { get; set; }
    }

    public class Commitid
    {
        [JsonProperty("@id")]
        public string id { get; set; }
    }

    public class Count
    {
        [JsonProperty("@id")]
        public string id { get; set; }
    }

    public class Parent
    {
        [JsonProperty("@id")]
        public string id { get; set; }
        [JsonProperty("@type")]
        public string type { get; set; }
    }

    public class Tags
    {
        public string container { get; set; }
        [JsonProperty("@id")]
        public string id { get; set; }
    }

    public class Reasons
    {
        public string container { get; set; }
    }

    public class Packagetargetframeworks
    {
        public string container { get; set; }
        [JsonProperty("@id")]
        public string id { get; set; }
    }

    public class Dependencygroups
    {
        public string container { get; set; }
        [JsonProperty("@id")]
        public string id { get; set; }
    }

    public class Dependencies
    {
        public string container { get; set; }
        [JsonProperty("@id")]
        public string id { get; set; }
    }

    public class Packagecontent
    {
        [JsonProperty("@type")]
        public string type { get; set; }
    }

    public class Published
    {
        [JsonProperty("@type")]
        public string type { get; set; }
    }

    public class Registration
    {
        [JsonProperty("@type")]
        public string type { get; set; }
    }

    public class Item
    {
        [JsonProperty("@id")]
        public string id { get; set; }
        [JsonProperty("@type")]
        public string type { get; set; }
        public string commitId { get; set; }
        public DateTime commitTimeStamp { get; set; }
        public int count { get; set; }
        public Item1[] items { get; set; }
        public string parent { get; set; }
        public string lower { get; set; }
        public string upper { get; set; }
    }

    public class Item1
    {
        [JsonProperty("@id")]
        public string id { get; set; }
        [JsonProperty("@type")]
        public string type { get; set; }
        public string commitId { get; set; }
        public DateTime commitTimeStamp { get; set; }
        public Catalogentry catalogEntry { get; set; }
        public string packageContent { get; set; }
        public string registration { get; set; }
    }

    public class Catalogentry
    {
        [JsonProperty("@id")]
        public string id { get; set; }
        [JsonProperty("@type")]
        public string type { get; set; }
        public string authors { get; set; }
        public string description { get; set; }
        public string iconUrl { get; set; }
        public string language { get; set; }
        public string licenseExpression { get; set; }
        public string licenseUrl { get; set; }
        public bool listed { get; set; }
        public string minClientVersion { get; set; }
        public string packageContent { get; set; }
        public string projectUrl { get; set; }
        public DateTime published { get; set; }
        public bool requireLicenseAcceptance { get; set; }
        public string summary { get; set; }
        public string[] tags { get; set; }
        public string title { get; set; }
        public string version { get; set; }
        public Dependencygroup[] dependencyGroups { get; set; }
    }

    public class Dependencygroup
    {
        [JsonProperty("@id")]
        public string id { get; set; }
        [JsonProperty("@type")]
        public string type { get; set; }
        public string targetFramework { get; set; }
        public Dependency[] dependencies { get; set; }
    }

    public class Dependency
    {
        [JsonProperty("@id")]
        public string id { get; set; }
        [JsonProperty("@type")]
        public string type { get; set; }
        public string range { get; set; }
        public string registration { get; set; }
    }

}