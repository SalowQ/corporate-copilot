namespace CorporateCopilot.Api.Configuration;

/// <summary>
/// Opções de configuração da API Google Gemini.
/// </summary>
public sealed class GeminiOptions
{
    public const string SectionName = "Gemini";

    /// <summary>
    /// Chave de API do Google AI Studio (camada gratuita).
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Identificador do modelo Gemini (ex.: gemini-3.1-flash-lite).
    /// </summary>
    public string Model { get; set; } = "gemini-3.1-flash-lite";

    /// <summary>
    /// Endpoint base da API Generative Language.
    /// </summary>
    public string Endpoint { get; set; } = "https://generativelanguage.googleapis.com/v1beta";
}
