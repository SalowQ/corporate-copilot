using CorporateCopilot.Api.Models;

namespace CorporateCopilot.Api.Services.Interfaces;

/// <summary>
/// Lê a base de conhecimento em arquivos Markdown.
/// </summary>
public interface IKnowledgeBaseService
{
    Task<IReadOnlyList<KnowledgeDocument>> GetAllDocumentsAsync(CancellationToken cancellationToken = default);
}
