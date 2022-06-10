using Newtonsoft.Json;
using Pinax.Models.PackageManagers;

namespace Pinax.Services;

public static class PackageManagerService
{
    private const string NUGET_SERVICES_INDEX_URL =
        "https://api.nuget.org/v3/index.json";

    private static readonly NuGetServices? s_nuGetServices;

    static PackageManagerService()
    {
        s_nuGetServices = GetNuGetServices();
    }

    public static NuGetPackageVersions? GetNuGetPackageVersions(string packageName)
    {
        var serviceUri =
            s_nuGetServices?
                .Resources
                .First(r => r.Type.StartsWith("PackageBaseAddress"))
                .Id;

        string fullUriString =
            $"{serviceUri}{packageName.ToLowerInvariant()}/index.json";

        return GetDeserializedWebResponse<NuGetPackageVersions>(fullUriString);
    }

    private static NuGetServices? GetNuGetServices()
    {
        return GetDeserializedWebResponse<NuGetServices>(NUGET_SERVICES_INDEX_URL);
    }

    private static T? GetDeserializedWebResponse<T>(string uri)
    {
        using var httpClient = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, uri);

        HttpResponseMessage response = httpClient.Send(request);

        if (response.IsSuccessStatusCode)
        {
            using var reader = new StreamReader(response.Content.ReadAsStream());
            var responseBody = reader.ReadToEnd();

            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        return default(T);
    }

}