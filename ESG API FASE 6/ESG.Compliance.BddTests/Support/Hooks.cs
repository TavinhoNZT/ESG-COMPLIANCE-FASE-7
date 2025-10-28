using System;
using TechTalk.SpecFlow;

namespace ESG.Compliance.BddTests.Support;

[Binding]
public class Hooks
{
    [BeforeTestRun]
    public static void BeforeTestRun()
    {
        var baseUrl = Environment.GetEnvironmentVariable("TEST_BASE_URL");
        if (string.IsNullOrWhiteSpace(baseUrl)) baseUrl = "http://localhost:8080";
        Console.WriteLine($"[SpecFlow] Test Base URL: {baseUrl}");
    }
}