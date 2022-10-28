using Newtonsoft.Json;
using Pinax.Engine.Models;

namespace Pinax.Engine.Services;

public static class ServiceIndexReader
{
    public static ServiceIndex GetServiceIndex(string url)
    {
        using var webClient = new HttpClient();

        var jsonData = webClient.GetStringAsync(url).Result;

        return JsonConvert.DeserializeObject<ServiceIndex>(jsonData);
    }
}
