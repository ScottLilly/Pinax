using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Pinax.Engine.Models;
using System.Xml.Serialization;

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
        var packageSources = new List<NuGetPackageSource>();

        string fullFilePath =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "NuGet",
                "NuGet.config");

        try
        {
            if (File.Exists(fullFilePath))
            {
                XmlSerializer xmlSerSale =
                    new XmlSerializer(typeof(NuGetConfig.configuration));

                StringReader stringReader =
                    new StringReader(File.ReadAllText(fullFilePath));

                var info =
                    (NuGetConfig.configuration)xmlSerSale.Deserialize(stringReader);

                packageSources.AddRange(
                    info.packageSources
                        .Where(ps => ps.IsWebSource)
                        .Select(ps => new NuGetPackageSource
                        {
                            Url = ps.value, 
                            ProtocolVersion = ps.protocolVersion
                        }));

                stringReader.Close();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error reading NuGet.config file at: {fullFilePath}");
        }

        if (packageSources.None())
        {
            packageSources.Add(
                new NuGetPackageSource
                {
                    Url = "https://api.nuget.org/v3/index.json",
                    ProtocolVersion = 3
                });
        }

        return packageSources;
    }
}