namespace CorporateCopilot.Api.Configuration;

/// <summary>
/// Origens permitidas para CORS (frontend local e produção).
/// </summary>
public sealed class CorsSettings
{
    public const string SectionName = "Cors";

    /// <summary>
    /// Lista de origens autorizadas a chamar a API.
    /// </summary>
    public string[] AllowedOrigins { get; set; } =
    [
        "http://localhost:5173",
        "http://127.0.0.1:5173"
    ];
}
