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
    [Route("api/desenvolvedora")]
    [ApiController]
    public class DesenvolvedoraController : ControllerBase
    {
        private readonly IDesenvolvedoraUseCase _desenvolvedoraUseCase;
        private readonly ILogger<DesenvolvedoraController> _logger;

        public DesenvolvedoraController(IDesenvolvedoraUseCase desenvolvedoraUseCase, ILogger<DesenvolvedoraController> logger)
        {
            _desenvolvedoraUseCase = desenvolvedoraUseCase;
            _logger = logger;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista todas as desenvolvedoras",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista paginada de desenvolvedoras.
            * **Status 204 (No Content):** Executado com sucesso, porém não há desenvolvedoras cadastradas.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<DesenvolvedoraResponseDto>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma desenvolvedora encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(DesenvolvedoraResponseListSample))]
        [EnableRateLimiting("politica_5_tentativas")]
        public async Task<IActionResult> Get(int Deslocamento = 0, int RegistroRetornado = 50)
        {
            _logger.LogInformation("Listando desenvolvedoras, Deslocamento={Deslocamento}, RegistroRetornado={RegistroRetornado}", Deslocamento, RegistroRetornado);

            try
            {
                var resultado = await _desenvolvedoraUseCase.ObterTodosDesenvolvedorasAsync(Deslocamento, RegistroRetornado);

                if (!resultado.Data.Any())
                    return NoContent();

                return Ok(resultado.ToResponseDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar desenvolvedoras");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Obter desenvolvedora por id",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna a desenvolvedora localizada.
            * **Status 404 (Not Found):** Não foi encontrada desenvolvedora com o id informado.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Desenvolvedora retornada com sucesso", type: typeof(DesenvolvedoraResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Desenvolvedora não encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(DesenvolvedoraResponseSample))]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("Obtendo desenvolvedora {DesenvolvedoraId}", id);

            try
            {
                var desenvolvedora = await _desenvolvedoraUseCase.ObterUmaDesenvolvedoraAsync(id);

                if (desenvolvedora is null)
                {
                    _logger.LogWarning("Desenvolvedora {DesenvolvedoraId} não encontrada", id);
                    return NotFound();
                }

                return Ok(desenvolvedora.ToResponseDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter desenvolvedora {DesenvolvedoraId}", id);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Adicionar desenvolvedora",
            Description = """
            ## Informações do Retorno:
            * **Status 201 (Created):** Desenvolvedora criada com sucesso.
            * **Status 409 (Conflict):** Já existe uma desenvolvedora com esse nome.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao criar a desenvolvedora (ex: dados inválidos).
            """
        )]
        [SwaggerRequestExample(typeof(DesenvolvedoraRequestDto), typeof(DesenvolvedoraRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Desenvolvedora criada com sucesso", type: typeof(DesenvolvedoraResponseDto))]
        [SwaggerResponse(statusCode: 409, description: "Já existe uma desenvolvedora com esse nome", type: typeof(string))]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao criar a desenvolvedora", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 201, typeof(DesenvolvedoraCreatedSample))]
        public async Task<IActionResult> Post(DesenvolvedoraRequestDto model)
        {
            _logger.LogInformation("Criando desenvolvedora {Nome}", model.Nome);

            try
            {
                var desenvolvedora = await _desenvolvedoraUseCase.AdicionarDesenvolvedoraAsync(model);

                if (desenvolvedora is null)
                {
                    _logger.LogWarning("Desenvolvedora {Nome} já existe", model.Nome);
                    return Conflict($"Já existe uma desenvolvedora com o nome '{model.Nome}'.");
                }

                return CreatedAtAction(nameof(Get), new { id = desenvolvedora.Id }, desenvolvedora.ToResponseDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar desenvolvedora {Nome}", model.Nome);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Editar desenvolvedora",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Desenvolvedora editada com sucesso.
            * **Status 404 (Not Found):** Não foi encontrada desenvolvedora com o id informado.
            * **Status 409 (Conflict):** Já existe uma desenvolvedora com esse nome.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao editar a desenvolvedora.
            """
        )]
        [SwaggerRequestExample(typeof(DesenvolvedoraRequestDto), typeof(DesenvolvedoraRequestSample))]
        [SwaggerResponse(statusCode: 200, description: "Desenvolvedora editada com sucesso", type: typeof(DesenvolvedoraResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Desenvolvedora não encontrada")]
        [SwaggerResponse(statusCode: 409, description: "Já existe uma desenvolvedora com esse nome", type: typeof(string))]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao editar a desenvolvedora", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(DesenvolvedoraResponseSample))]
        public async Task<IActionResult> Put(int id, DesenvolvedoraRequestDto model)
        {
            _logger.LogInformation("Editando desenvolvedora {DesenvolvedoraId}", id);

            try
            {
                var desenvolvedora = await _desenvolvedoraUseCase.EditarDesenvolvedoraAsync(id, model);

                if (desenvolvedora is null)
                {
                    _logger.LogWarning("Desenvolvedora {DesenvolvedoraId} não encontrada para edição", id);
                    return NotFound();
                }

                return Ok(desenvolvedora.ToResponseDto());
            }
            catch (NomeDuplicadoException ex)
            {
                _logger.LogWarning(ex, "Nome duplicado ao editar desenvolvedora {DesenvolvedoraId}", id);
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao editar desenvolvedora {DesenvolvedoraId}", id);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Deletar desenvolvedora",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Desenvolvedora deletada com sucesso.
            * **Status 404 (Not Found):** Não foi encontrada desenvolvedora com o id informado.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao deletar a desenvolvedora.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Desenvolvedora deletada com sucesso", type: typeof(DesenvolvedoraResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Desenvolvedora não encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao deletar a desenvolvedora", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(DesenvolvedoraResponseSample))]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deletando desenvolvedora {DesenvolvedoraId}", id);

            try
            {
                var desenvolvedora = await _desenvolvedoraUseCase.DeletarDesenvolvedoraAsync(id);

                if (desenvolvedora is null)
                {
                    _logger.LogWarning("Desenvolvedora {DesenvolvedoraId} não encontrada para deleção", id);
                    return NotFound();
                }

                return Ok(desenvolvedora.ToResponseDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar desenvolvedora {DesenvolvedoraId}", id);
                return BadRequest(ex.Message);
            }
        }
    }
}
