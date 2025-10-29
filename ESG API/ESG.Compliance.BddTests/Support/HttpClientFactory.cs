using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ESG.Compliance.BddTests.Support;

public static class HttpClientFactoryESG
{
    public static HttpClient Create()
    {
        var baseUrl = Environment.GetEnvironmentVariable("TEST_BASE_URL");
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            baseUrl = "http://localhost:8081";
        }

        var client = new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return client;
    }
}