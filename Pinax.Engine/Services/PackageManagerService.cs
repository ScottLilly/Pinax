using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Pinax.Engine.Models;

namespace Pinax.Engine.Services;

public static class PackageManagerService
{
    private static readonly SourceCacheContext s_cache = new();
    private static readonly List<SourceRepository> s_repositories = new();

    static PackageManagerService()
    {
        var sources = GetNuGetSources();

        foreach (var source in sources)
        {
            switch (source.ProtocolVersion)
            {
                case 2:
                    s_repositories.Add(
                        Repository.Factory.GetCoreV2(new PackageSource(source.Url)));
                    break;
                case 3:
                    s_repositories.Add(
                        Repository.Factory.GetCoreV3(source.Url));
                    break;
            }
        }
    }

    public static async Task<List<NuGetVersion>> GetPackageVersions(string packageName)
    {
        FindPackageByIdResource? resource = null;

        foreach (var repository in s_repositories)
        {
            resource =
                await repository.GetResourceAsync<FindPackageByIdResource>()
                    .ConfigureAwait(false);

            if (resource != null)
            {
                break;
            }
        }

        if (resource == null)
        {
            return new List<NuGetVersion>();
        }

        IEnumerable<NuGetVersion> versions =
            resource.GetAllVersionsAsync(
            packageName,
            s_cache,
            NullLogger.Instance,
            CancellationToken.None).Result;

        return versions.Where(v => !v.IsPrerelease).ToList();
    }

    private static List<NuGetPackageSource> GetNuGetSources()
    {
        var sources = new List<NuGetPackageSource>();


        if (sources.None())
        {
            sources.Add(new NuGetPackageSource()
            {
                Url = "https://api.nuget.org/v3/index.json", 
                ProtocolVersion = 3
            });
        }
        
        return sources;
    }
}