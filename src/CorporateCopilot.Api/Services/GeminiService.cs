using System.Net.Http.Json;
using System.Text.Json;
using CorporateCopilot.Api.Configuration;
using CorporateCopilot.Api.Models.Gemini;
using CorporateCopilot.Api.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace CorporateCopilot.Api.Services;

/// <summary>
/// Cliente HTTP para a API Google Gemini (camada gratuita).
/// </summary>
public sealed class GeminiService : IGeminiService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly HttpClient _httpClient;
    private readonly GeminiOptions _options;
    private readonly ILogger<GeminiService> _logger;

    public GeminiService(
        HttpClient httpClient,
        IOptions<GeminiOptions> options,
        ILogger<GeminiService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<string> GenerateAsync(string prompt, CancellationToken cancellationToken = default)
    {
        EnsureConfigured();

        var relativeUrl = BuildRelativeUrl();
        var payload = CreatePayload(prompt);

        _logger.LogInformation(
            "Enviando solicitação ao Gemini. Modelo: {Model}. ApiKey configurada: {HasKey} (tamanho {KeyLength}). URL relativa: {RelativeUrl}",
            _options.Model,
            !string.IsNullOrWhiteSpace(_options.ApiKey),
            _options.ApiKey.Length,
            MaskKeyInUrl(relativeUrl));

        using var response = await _httpClient.PostAsJsonAsync(
            relativeUrl,
            payload,
            JsonOptions,
            cancellationToken);

        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var geminiError = TryReadErrorMessage(body);

            _logger.LogError(
                "Falha na API Gemini. Status: {StatusCode}. Detalhe: {Detail}. Corpo: {Body}",
                (int)response.StatusCode,
                geminiError ?? "sem detalhe",
                Truncate(body, 500));

            throw new InvalidOperationException(
                geminiError is not null
                    ? $"A API Gemini retornou erro HTTP {(int)response.StatusCode}: {geminiError}"
                    : $"A API Gemini retornou erro HTTP {(int)response.StatusCode}.");
        }

        var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(body, JsonOptions)
            ?? throw new InvalidOperationException("Não foi possível interpretar a resposta do Gemini.");

        if (!string.IsNullOrWhiteSpace(geminiResponse.Error?.Message))
        {
            throw new InvalidOperationException(
                $"Erro retornado pelo Gemini: {geminiResponse.Error.Message}");
        }

        var answer = ExtractAnswer(geminiResponse);

        if (string.IsNullOrWhiteSpace(answer))
        {
            throw new InvalidOperationException("O Gemini não retornou conteúdo textual.");
        }

        return answer.Trim();
    }

    private void EnsureConfigured()
    {
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            throw new InvalidOperationException(
                "A chave Gemini não está configurada. Defina Gemini:ApiKey (user-secrets) ou a variável Gemini__ApiKey.");
        }

        if (string.IsNullOrWhiteSpace(_options.Model))
        {
            throw new InvalidOperationException("O modelo Gemini não está configurado.");
        }

        if (string.IsNullOrWhiteSpace(_options.Endpoint))
        {
            throw new InvalidOperationException("O endpoint Gemini não está configurado.");
        }

        if (_httpClient.BaseAddress is null)
        {
            throw new InvalidOperationException("O HttpClient do Gemini não possui BaseAddress configurado.");
        }
    }

    /// <summary>
    /// Formato oficial da documentação:
    /// POST /v1beta/models/{model}:generateContent?key=API_KEY
    /// </summary>
    private string BuildRelativeUrl()
    {
        var model = _options.Model.Trim();
        var key = Uri.EscapeDataString(_options.ApiKey.Trim());
        return $"models/{model}:generateContent?key={key}";
    }

    private static GeminiRequest CreatePayload(string prompt)
    {
        return new GeminiRequest
        {
            Contents =
            [
                new GeminiContent
                {
                    Parts =
                    [
                        new GeminiPart { Text = prompt }
                    ]
                }
            ]
        };
    }

    private static string? ExtractAnswer(GeminiResponse response)
    {
        return response.Candidates?
            .SelectMany(candidate => candidate.Content?.Parts ?? [])
            .Select(part => part.Text)
            .FirstOrDefault(text => !string.IsNullOrWhiteSpace(text));
    }

    private static string? TryReadErrorMessage(string body)
    {
        try
        {
            using var document = JsonDocument.Parse(body);

            if (document.RootElement.TryGetProperty("error", out var error) &&
                error.TryGetProperty("message", out var message))
            {
                return message.GetString();
            }
        }
        catch (JsonException)
        {
            // Mantém mensagem genérica.
        }

        return null;
    }

    private static string MaskKeyInUrl(string url)
    {
        var index = url.IndexOf("key=", StringComparison.OrdinalIgnoreCase);
        if (index < 0) return url;
        return string.Concat(url.AsSpan(0, index + 4), "***");
    }

    private static string Truncate(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
        {
            return value;
        }

        return value[..maxLength] + "...";
    }
}
