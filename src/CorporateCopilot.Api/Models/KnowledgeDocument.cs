namespace CorporateCopilot.Api.Models;

/// <summary>
/// Documento da base de conhecimento interna.
/// </summary>
public sealed class KnowledgeDocument
{
    public required string FileName { get; init; }

    public required string Content { get; init; }
}
