using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Json.Schema;
using TechTalk.SpecFlow;
using ESG.Compliance.BddTests.Support;

namespace ESG.Compliance.BddTests.StepDefinitions;

[Binding]
public class ApiStepDefinitions
{
    private HttpClient? _client;
    private HttpResponseMessage? _response;
    private string? _payload;
    private string? _lastLocationUrl;

    [Given(@"a base URL configurada")]
    public void GivenABaseUrlConfigurada()
    {
        _client = HttpClientFactoryESG.Create();
        _client.Should().NotBeNull();
        _client!.BaseAddress.Should().NotBeNull();
    }

    [Given(@"o payload JSON:")]
    public void GivenOPayloadJson(string payload)
    {
        _payload = payload;
        _payload.Should().NotBeNullOrWhiteSpace();
    }

    [Then(@"o payload deve obedecer ao schema ""(.*)""")]
    public void ThenOPayloadDeveObedecerAoSchema(string schemaFileName)
    {
        _payload.Should().NotBeNullOrWhiteSpace();
        var schema = LoadSchema(schemaFileName);
        using var doc = JsonDocument.Parse(_payload!);
        var eval = schema.Evaluate(doc.RootElement, new EvaluationOptions { OutputFormat = OutputFormat.Hierarchical });
        eval.IsValid.Should().BeTrue(GetErrors(eval));
    }

    [When(@"eu faço GET em ""\{url\}""")]
    public async Task WhenEuFacoGETEmUrlPlaceholder()
    {
        _client.Should().NotBeNull();
        _lastLocationUrl.Should().NotBeNullOrWhiteSpace("precisa extrair o Location antes");
        var url = _lastLocationUrl!;
        _response = await _client!.GetAsync(url);
    }

    [When(@"eu faço GET em ""(.*)""")]
    public async Task WhenEuFacoGETEm(string rota)
    {
        _client.Should().NotBeNull();
        var url = BuildUrl(rota);
        _response = await _client!.GetAsync(url);
    }

    [When(@"eu faço POST em ""(.*)"" com o payload")]
    public async Task WhenEuFacoPOSTEmComOPayload(string rota)
    {
        _client.Should().NotBeNull();
        _payload.Should().NotBeNullOrWhiteSpace();
        var url = BuildUrl(rota);
        var content = new StringContent(_payload!, Encoding.UTF8, "application/json");
        _response = await _client!.PostAsync(url, content);
    }

    [Then(@"o status deve ser (\d+)")]
    public void ThenOStatusDeveSer(int statusCode)
    {
        _response.Should().NotBeNull();
        ((int)_response!.StatusCode).Should().Be(statusCode);
    }

    [Then(@"o corpo deve ser JSON válido")]
    public async Task ThenOCorpoDeveSerJsonValido()
    {
        _response.Should().NotBeNull();
        var body = await _response!.Content.ReadAsStringAsync();
        using var _ = JsonDocument.Parse(body);
    }

    [Then(@"o corpo deve ser um array JSON")]
    public async Task ThenOCorpoDeveSerUmArrayJson()
    {
        _response.Should().NotBeNull();
        var body = await _response!.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        doc.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
    }

    [Then(@"o header ""Location"" deve existir")]
    public void ThenOHeaderLocationDeveExistir()
    {
        _response.Should().NotBeNull();
        var hasLocation = _response!.Headers.Location != null
                          || (_response.Headers.TryGetValues("Location", out var values) && values.Any());
        hasLocation.Should().BeTrue("a resposta deve conter o header Location");
    }

    [Then(@"o corpo deve obedecer ao schema ""(.*)""")]
    public async Task ThenOCorpoDeveObedecerAoSchema(string schemaFileName)
    {
        _response.Should().NotBeNull();
        var body = await _response!.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        var schema = LoadSchema(schemaFileName);
        var eval = schema.Evaluate(doc.RootElement, new EvaluationOptions { OutputFormat = OutputFormat.Hierarchical });
        eval.IsValid.Should().BeTrue(GetErrors(eval));
    }

    [Then(@"eu extraio o ""Location"" como url do novo recurso")]
    public void ThenEuExtraioOLocationComoUrlDoNovoRecurso()
    {
        _response.Should().NotBeNull();
        string? locationStr = _response!.Headers.Location?.ToString();
        if (string.IsNullOrWhiteSpace(locationStr))
        {
            if (_response.Headers.TryGetValues("Location", out var values))
                locationStr = values.FirstOrDefault();
        }
        locationStr.Should().NotBeNullOrWhiteSpace("Location deve existir");

        if (Uri.TryCreate(locationStr!, UriKind.Absolute, out var absolute))
        {
            _lastLocationUrl = absolute.ToString();
        }
        else
        {
            _client.Should().NotBeNull();
            var baseUri = _client!.BaseAddress!;
            var relative = NormalizeRoute(locationStr!);
            var resolved = new Uri(baseUri, relative);
            _lastLocationUrl = resolved.ToString();
        }
    }

    [Then(@"o corpo deve conter os campos ""(.*)""")]
    public async Task ThenOCorpoDeveConterOsCampos(string campos)
    {
        _response.Should().NotBeNull();
        var body = await _response!.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        doc.RootElement.ValueKind.Should().Be(JsonValueKind.Object);
        var props = campos.Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToArray();
        foreach (var prop in props)
        {
            doc.RootElement.TryGetProperty(prop, out var _).Should().BeTrue($"campo '{prop}' deve existir no corpo");
        }
    }

    private string BuildUrl(string rota)
    {
        if (string.IsNullOrWhiteSpace(rota)) throw new ArgumentException("rota inválida", nameof(rota));
        // rota absoluta
        if (Uri.TryCreate(rota, UriKind.Absolute, out var abs)) return abs.ToString();
        // normaliza rota relativa
        var normalized = NormalizeRoute(rota);
        _client.Should().NotBeNull();
        var baseUri = _client!.BaseAddress!;
        return new Uri(baseUri, normalized).ToString();
    }

    private static string NormalizeRoute(string rota)
    {
        if (string.IsNullOrWhiteSpace(rota)) return "/";
        return rota.StartsWith("/") ? rota : "/" + rota;
    }

    private static JsonSchema LoadSchema(string schemaFileName)
    {
        // Garante extensão .json e diretório correto
        var file = schemaFileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase)
            ? schemaFileName
            : schemaFileName + ".json";
        var path = Path.Combine(AppContext.BaseDirectory, "Support", "JsonSchemas", file);
        if (!File.Exists(path))
        {
            // fallback: tentar subir pastas (em caso de execução fora do bin)
            var alt = Path.Combine(AppContext.BaseDirectory, "..", "..", "Support", "JsonSchemas", file);
            path = Path.GetFullPath(alt);
        }
        File.Exists(path).Should().BeTrue($"Schema não encontrado em '{path}'");
        var schemaText = File.ReadAllText(path);
        return JsonSchema.FromText(schemaText);
    }

    private static string GetErrors(EvaluationResults eval)
    {
        if (eval.IsValid) return string.Empty;
        try
        {
            var details = eval.Details ?? Array.Empty<EvaluationResults>();
            var errors = details
                .Where(d => d.HasErrors)
                .SelectMany(d => d.Errors.Select(e => $"{d.InstanceLocation}: {e.Key} -> {e.Value}"));
            return "Schema validation failed:\n" + string.Join("\n", errors);
        }
        catch
        {
            return "Schema validation failed (no detailed errors available).";
        }
    }
}