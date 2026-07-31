using CorporateCopilot.Api.Models;

namespace CorporateCopilot.Api.Services.Interfaces;

/// <summary>
/// Monta o prompt enviado ao modelo generativo.
/// </summary>
public interface IPromptBuilderService
{
    string Build(IReadOnlyList<KnowledgeDocument> documents, string question);
}
