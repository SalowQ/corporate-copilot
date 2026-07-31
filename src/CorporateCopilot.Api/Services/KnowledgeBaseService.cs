using CorporateCopilot.Api.Models;
using CorporateCopilot.Api.Services.Interfaces;

namespace CorporateCopilot.Api.Services;

/// <summary>
/// Carrega documentos Markdown da pasta KnowledgeBase.
/// </summary>
public sealed class KnowledgeBaseService : IKnowledgeBaseService
{
    private readonly ILogger<KnowledgeBaseService> _logger;
    private readonly string _knowledgeBasePath;
    private IReadOnlyList<KnowledgeDocument>? _cache;
    private readonly SemaphoreSlim _lock = new(1, 1);

    public KnowledgeBaseService(IHostEnvironment environment, ILogger<KnowledgeBaseService> logger)
    {
        _logger = logger;
        _knowledgeBasePath = Path.Combine(environment.ContentRootPath, "KnowledgeBase");
    }

    public async Task<IReadOnlyList<KnowledgeDocument>> GetAllDocumentsAsync(
        CancellationToken cancellationToken = default)
    {
        if (_cache is not null)
        {
            return _cache;
        }

        await _lock.WaitAsync(cancellationToken);

        try
        {
            if (_cache is not null)
            {
                return _cache;
            }

            _cache = await LoadDocumentsAsync(cancellationToken);
            return _cache;
        }
        finally
        {
            _lock.Release();
        }
    }

    private async Task<IReadOnlyList<KnowledgeDocument>> LoadDocumentsAsync(
        CancellationToken cancellationToken)
    {
        if (!Directory.Exists(_knowledgeBasePath))
        {
            _logger.LogWarning("Pasta da base de conhecimento não encontrada: {Path}", _knowledgeBasePath);
            return [];
        }

        var files = Directory.GetFiles(_knowledgeBasePath, "*.md", SearchOption.TopDirectoryOnly);
        var documents = new List<KnowledgeDocument>(files.Length);

        foreach (var file in files.OrderBy(Path.GetFileName, StringComparer.OrdinalIgnoreCase))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var content = await File.ReadAllTextAsync(file, cancellationToken);
            documents.Add(new KnowledgeDocument
            {
                FileName = Path.GetFileName(file),
                Content = content
            });
        }

        _logger.LogInformation("Base de conhecimento carregada com {Count} documento(s).", documents.Count);
        return documents;
    }
}
