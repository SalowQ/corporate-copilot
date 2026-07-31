using System.Reflection;
using CorporateCopilot.Api.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CorporateCopilot.Api.Controllers;

/// <summary>
/// Endpoints de saúde da aplicação.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public sealed class HealthController : ControllerBase
{
    /// <summary>
    /// Retorna o status atual da API.
    /// </summary>
    /// <returns>Status, versão e timestamp UTC.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(HealthResponse), StatusCodes.Status200OK)]
    public ActionResult<HealthResponse> Get()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0";

        return Ok(new HealthResponse
        {
            Status = "Healthy",
            Version = version,
            Timestamp = DateTimeOffset.UtcNow
        });
    }
}
