using CorporateCopilot.Api.Models.Requests;
using CorporateCopilot.Api.Models.Responses;
using CorporateCopilot.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CorporateCopilot.Api.Controllers;

/// <summary>
/// Endpoints de conversa com o assistente corporativo.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public sealed class ChatController : ControllerBase
{
    private readonly IKnowledgeBaseService _knowledgeBaseService;
    private readonly IPromptBuilderService _promptBuilderService;
    private readonly IGeminiService _geminiService;
    private readonly ILogger<ChatController> _logger;

    public ChatController(
        IKnowledgeBaseService knowledgeBaseService,
        IPromptBuilderService promptBuilderService,
        IGeminiService geminiService,
        ILogger<ChatController> logger)
    {
        _knowledgeBaseService = knowledgeBaseService;
        _promptBuilderService = promptBuilderService;
        _geminiService = geminiService;
        _logger = logger;
    }

    /// <summary>
    /// Responde uma pergunta com base nos documentos internos da empresa.
    /// </summary>
    /// <param name="request">Pergunta do colaborador.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Resposta gerada pelo Gemini a partir da base de conhecimento.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ChatResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<ActionResult<ChatResponse>> Post(
        [FromBody] ChatRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var message = request.Message.Trim();

        if (string.IsNullOrWhiteSpace(message))
        {
            return BadRequest(new { detail = "A mensagem não pode ser vazia." });
        }

        _logger.LogInformation("Recebida pergunta ao CorporateCopilot.");

        var documents = await _knowledgeBaseService.GetAllDocumentsAsync(cancellationToken);
        var prompt = _promptBuilderService.Build(documents, message);
        var answer = await _geminiService.GenerateAsync(prompt, cancellationToken);

        return Ok(new ChatResponse { Answer = answer });
    }
}
