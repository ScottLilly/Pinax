namespace Pinax.Models;

public class Package
{
    public string Name { get; set; }
    public Version Version { get; set; }
    public string TargetFramework { get; set; }

    public override string ToString() =>
        $"{Name} {Version}";
}