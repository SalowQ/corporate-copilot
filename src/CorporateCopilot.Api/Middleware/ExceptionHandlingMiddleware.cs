using System.Net;
using System.Text.Json;

namespace CorporateCopilot.Api.Middleware;

/// <summary>
/// Middleware global de tratamento de exceções.
/// </summary>
public sealed class ExceptionHandlingMiddleware
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            context.Response.StatusCode = 499;
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "Erro não tratado na requisição {Path}", context.Request.Path);

        var statusCode = exception switch
        {
            InvalidOperationException => (int)HttpStatusCode.BadGateway,
            ArgumentException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var problem = new
        {
            type = "https://httpstatuses.com/" + statusCode,
            title = GetTitle(statusCode),
            status = statusCode,
            detail = exception.Message,
            traceId = context.TraceIdentifier
        };

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsync(JsonSerializer.Serialize(problem, JsonOptions));
    }

    private static string GetTitle(int statusCode) => statusCode switch
    {
        (int)HttpStatusCode.BadRequest => "Requisição inválida",
        (int)HttpStatusCode.BadGateway => "Falha na integração com o Gemini",
        _ => "Erro interno do servidor"
    };
}
