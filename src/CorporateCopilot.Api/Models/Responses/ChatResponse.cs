namespace CorporateCopilot.Api.Models.Responses;

/// <summary>
/// Resposta do assistente corporativo.
/// </summary>
public sealed class ChatResponse
{
    /// <summary>
    /// Resposta gerada com base na base de conhecimento.
    /// </summary>
    public required string Answer { get; init; }
}
