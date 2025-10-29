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
            Console.WriteLine($"[BDD] TEST_BASE_URL não definida, usando fallback: {baseUrl}");
        }
        else
        {
            Console.WriteLine($"[BDD] TEST_BASE_URL definida: {baseUrl}");
        }

        var client = new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };
        
        Console.WriteLine($"[BDD] HttpClient criado com BaseAddress: {client.BaseAddress}");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return client;
    }
}