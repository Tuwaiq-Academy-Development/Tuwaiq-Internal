using System.Net;
using Newtonsoft.Json.Linq;

namespace TuwaiqInternal.Helper;

public class GoogleReCaptcha : IGoogleReCaptcha
{
    private readonly HttpClient _httpClient;

    public GoogleReCaptcha(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("google");
    }
    
    public bool ReCaptchaPassedV2(string gRecaptchaResponse)
    {
        var res = _httpClient
            .GetAsync(
                $"siteverify?secret=6Lc-IispAAAAAEn2C1pwQjwv-tnapYzBmPZaWZbE&response={gRecaptchaResponse}")
            .Result;

        if (res.StatusCode != HttpStatusCode.OK)
        {
            return false;
        }

        string jsoNres = res.Content.ReadAsStringAsync().Result;
        dynamic jsoNdata = JObject.Parse(jsoNres);

        if (jsoNdata.success != "true" || jsoNdata.score <= 0.5m)
        {
            return false;
        }

        return true;
    }
}