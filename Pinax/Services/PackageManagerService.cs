using NuGet.Common;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace Pinax.Services;

public static class PackageManagerService
{
    private static readonly SourceCacheContext s_cache = new();
    private static readonly SourceRepository s_repository =
        Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");

    public static async Task<List<NuGetVersion>> GetPackageVersions(string packageName)
    {
        FindPackageByIdResource resource =
            await s_repository.GetResourceAsync<FindPackageByIdResource>()
                .ConfigureAwait(false);

        IEnumerable<NuGetVersion> versions =
            resource.GetAllVersionsAsync(
            packageName,
            s_cache,
            NullLogger.Instance,
            CancellationToken.None).Result;

        return versions.Where(v => !v.IsPrerelease).ToList();
    }
}