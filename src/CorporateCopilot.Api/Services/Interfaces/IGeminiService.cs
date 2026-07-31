namespace CorporateCopilot.Api.Services.Interfaces;

/// <summary>
/// Integração com a API Google Gemini.
/// </summary>
public interface IGeminiService
{
    Task<string> GenerateAsync(string prompt, CancellationToken cancellationToken = default);
}
