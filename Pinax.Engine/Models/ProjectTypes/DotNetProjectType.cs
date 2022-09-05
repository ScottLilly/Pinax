using Newtonsoft.Json;

namespace Pinax.Engine.Models.ProjectTypes;

public class DotNetProjectType : IProjectType<Enums.DotNetType>
{
    [JsonIgnore]
    public Enums.DotNetType Type { get; }
    [JsonIgnore]
    public Version Version { get; }

    public string DotNetVersion =>
        $"{Enum.GetName(typeof(Enums.DotNetType), Type)} {Version}";

    public DotNetProjectType(Enums.DotNetType dotNetType, Version version)
    {
        Type = dotNetType;
        Version = version;
    }

    public override string ToString()
    {
        return $"{Type.GetEnumDisplayName()} {Version}";
    }
}