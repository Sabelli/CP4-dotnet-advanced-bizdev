using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Swashbuckle.AspNetCore.Annotations;

namespace Jogos.API.Presentation.Controllers
{
    [Route("api/health2")]
    [ApiController]
    [AllowAnonymous]
    public class HealthController : ControllerBase
    {
        private readonly HealthCheckService _healthService;

        public HealthController(HealthCheckService healthService)
        {
            _healthService = healthService;
        }

        [HttpGet("live")]
        [SwaggerOperation(
            Summary = "Verifica se a API está no ar",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** API está viva (liveness).
            * **Status 503 (Service Unavailable):** API não está saudável.

            ## Observações:
            * Não valida dependências externas (banco de dados), apenas o processo da API.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "API está viva")]
        [SwaggerResponse(statusCode: 503, description: "API não está saudável")]
        public async Task<IActionResult> Live(CancellationToken ct)
        {
            var report = await _healthService.CheckHealthAsync(
                r => r.Tags.Contains("live"), ct);

            var result = new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(e => new
                {
                    name = e.Key,
                    status = e.Value.Status.ToString(),
                    description = e.Value.Description,
                    error = e.Value.Exception?.Message
                })
            };

            return report.Status == HealthStatus.Healthy
                ? Ok(result)
                : StatusCode(StatusCodes.Status503ServiceUnavailable, result);
        }

        [HttpGet("db")]
        [SwaggerOperation(
            Summary = "Verifica se o banco de dados está acessível",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Banco de dados Oracle acessível (readiness).
            * **Status 503 (Service Unavailable):** Banco de dados inacessível ou instável.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Banco de dados acessível")]
        [SwaggerResponse(statusCode: 503, description: "Banco de dados inacessível")]
        public async Task<IActionResult> Db(CancellationToken ct)
        {
            var report = await _healthService.CheckHealthAsync(
                r => r.Tags.Contains("db"), ct);

            var result = new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(e => new
                {
                    name = e.Key,
                    status = e.Value.Status.ToString(),
                    description = e.Value.Description,
                    error = e.Value.Exception?.Message
                })
            };

            return report.Status == HealthStatus.Healthy
                ? Ok(result)
                : StatusCode(StatusCodes.Status503ServiceUnavailable, result);
        }
    }
}
