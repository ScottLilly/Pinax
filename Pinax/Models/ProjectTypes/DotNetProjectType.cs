namespace Pinax.Models.ProjectTypes;

public class DotNetProjectType : IProjectType<Enums.DotNetType>
{
    public Enums.DotNetType Type { get; }
    public Version Version { get; }

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