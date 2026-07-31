using System.ComponentModel.DataAnnotations;

namespace CorporateCopilot.Api.Models.Requests;

/// <summary>
/// Solicitação de pergunta ao assistente corporativo.
/// </summary>
public sealed class ChatRequest
{
    /// <summary>
    /// Pergunta do usuário em linguagem natural.
    /// </summary>
    [Required(ErrorMessage = "A mensagem é obrigatória.")]
    [MinLength(3, ErrorMessage = "A mensagem deve ter pelo menos 3 caracteres.")]
    [MaxLength(2000, ErrorMessage = "A mensagem deve ter no máximo 2000 caracteres.")]
    public string Message { get; set; } = string.Empty;
}
