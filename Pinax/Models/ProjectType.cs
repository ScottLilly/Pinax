namespace Pinax.Models;

public class ProjectType
{
    public Enums.DotNetType DotNetType { get; }
    public Version Version { get; }

    public ProjectType(Enums.DotNetType dotNetType, Version version)
    {
        DotNetType = dotNetType;
        Version = version;
    }

    public override string ToString()
    {
        return $"{DotNetType.GetEnumDisplayName()} {Version}";
    }
}