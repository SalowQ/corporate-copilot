namespace CorporateCopilot.Api.Models.Responses;

/// <summary>
/// Status de saúde da API.
/// </summary>
public sealed class HealthResponse
{
    public required string Status { get; init; }

    public required string Version { get; init; }

    public required DateTimeOffset Timestamp { get; init; }
}
