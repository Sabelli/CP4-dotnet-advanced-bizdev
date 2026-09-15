using Jogos.API.Application.Dtos;
using Jogos.API.Application.Interfaces;
using Jogos.API.Application.Mappers;
using Jogos.API.Doc.Samples;
using Jogos.API.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Jogos.API.Presentation.Controllers
{
    [Route("api/plataforma")]
    [ApiController]
    public class PlataformaController : ControllerBase
    {
        private readonly IPlataformaUseCase _plataformaUseCase;
        private readonly ILogger<PlataformaController> _logger;

        public PlataformaController(IPlataformaUseCase plataformaUseCase, ILogger<PlataformaController> logger)
        {
            _plataformaUseCase = plataformaUseCase;
            _logger = logger;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista todas as plataformas",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista paginada de plataformas.
            * **Status 204 (No Content):** Executado com sucesso, porém não há plataformas cadastradas.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<PlataformaResponseDto>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma plataforma encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(PlataformaResponseListSample))]
        [EnableRateLimiting("politica_5_tentativas")]
        public async Task<IActionResult> Get(int Deslocamento = 0, int RegistroRetornado = 50)
        {
            _logger.LogInformation("Listando plataformas, Deslocamento={Deslocamento}, RegistroRetornado={RegistroRetornado}", Deslocamento, RegistroRetornado);

            try
            {
                var resultado = await _plataformaUseCase.ObterTodosPlataformasAsync(Deslocamento, RegistroRetornado);

                if (!resultado.Data.Any())
                    return NoContent();

                return Ok(resultado.ToResponseDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar plataformas");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Obter plataforma por id",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna a plataforma localizada.
            * **Status 404 (Not Found):** Não foi encontrada plataforma com o id informado.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Plataforma retornada com sucesso", type: typeof(PlataformaResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Plataforma não encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(PlataformaResponseSample))]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("Obtendo plataforma {PlataformaId}", id);

            try
            {
                var plataforma = await _plataformaUseCase.ObterUmaPlataformaAsync(id);

                if (plataforma is null)
                {
                    _logger.LogWarning("Plataforma {PlataformaId} não encontrada", id);
                    return NotFound();
                }

                return Ok(plataforma.ToResponseDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter plataforma {PlataformaId}", id);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Adicionar plataforma",
            Description = """
            ## Informações do Retorno:
            * **Status 201 (Created):** Plataforma criada com sucesso.
            * **Status 409 (Conflict):** Já existe uma plataforma com esse nome.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao criar a plataforma (ex: dados inválidos).
            """
        )]
        [SwaggerRequestExample(typeof(PlataformaRequestDto), typeof(PlataformaRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Plataforma criada com sucesso", type: typeof(PlataformaResponseDto))]
        [SwaggerResponse(statusCode: 409, description: "Já existe uma plataforma com esse nome", type: typeof(string))]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao criar a plataforma", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 201, typeof(PlataformaCreatedSample))]
        public async Task<IActionResult> Post(PlataformaRequestDto model)
        {
            _logger.LogInformation("Criando plataforma {Nome}", model.Nome);

            try
            {
                var plataforma = await _plataformaUseCase.AdicionarPlataformaAsync(model);

                if (plataforma is null)
                {
                    _logger.LogWarning("Plataforma {Nome} já existe", model.Nome);
                    return Conflict($"Já existe uma plataforma com o nome '{model.Nome}'.");
                }

                return CreatedAtAction(nameof(Get), new { id = plataforma.Id }, plataforma.ToResponseDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar plataforma {Nome}", model.Nome);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Editar plataforma",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Plataforma editada com sucesso.
            * **Status 404 (Not Found):** Não foi encontrada plataforma com o id informado.
            * **Status 409 (Conflict):** Já existe uma plataforma com esse nome.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao editar a plataforma.
            """
        )]
        [SwaggerRequestExample(typeof(PlataformaRequestDto), typeof(PlataformaRequestSample))]
        [SwaggerResponse(statusCode: 200, description: "Plataforma editada com sucesso", type: typeof(PlataformaResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Plataforma não encontrada")]
        [SwaggerResponse(statusCode: 409, description: "Já existe uma plataforma com esse nome", type: typeof(string))]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao editar a plataforma", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(PlataformaResponseSample))]
        public async Task<IActionResult> Put(int id, PlataformaRequestDto model)
        {
            _logger.LogInformation("Editando plataforma {PlataformaId}", id);

            try
            {
                var plataforma = await _plataformaUseCase.EditarPlataformaAsync(id, model);

                if (plataforma is null)
                {
                    _logger.LogWarning("Plataforma {PlataformaId} não encontrada para edição", id);
                    return NotFound();
                }

                return Ok(plataforma.ToResponseDto());
            }
            catch (NomeDuplicadoException ex)
            {
                _logger.LogWarning(ex, "Nome duplicado ao editar plataforma {PlataformaId}", id);
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao editar plataforma {PlataformaId}", id);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Deletar plataforma",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Plataforma deletada com sucesso.
            * **Status 404 (Not Found):** Não foi encontrada plataforma com o id informado.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao deletar a plataforma.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Plataforma deletada com sucesso", type: typeof(PlataformaResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Plataforma não encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao deletar a plataforma", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(PlataformaResponseSample))]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deletando plataforma {PlataformaId}", id);

            try
            {
                var plataforma = await _plataformaUseCase.DeletarPlataformaAsync(id);

                if (plataforma is null)
                {
                    _logger.LogWarning("Plataforma {PlataformaId} não encontrada para deleção", id);
                    return NotFound();
                }

                return Ok(plataforma.ToResponseDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar plataforma {PlataformaId}", id);
                return BadRequest(ex.Message);
            }
        }
    }
}
