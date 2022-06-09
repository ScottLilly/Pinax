using Newtonsoft.Json;

namespace Pinax.Models;

public class DotNetVersions
{
    public Version Standard { get; init; }
    public Version Framework { get; init; }
    public Version Core { get; init; }
    public Version DotNet { get; init; }
}