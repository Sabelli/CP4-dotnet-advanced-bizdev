using Jogos.API.Application.Dtos;
using Jogos.API.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;

namespace Jogos.API.Presentation.Controllers
{
    [Route("api/desenvolvedora")]
    [ApiController]
    public class DesenvolvedoraController : ControllerBase
    {
        private readonly IDesenvolvedoraUseCase _desenvolvedoraUseCase;

        public DesenvolvedoraController(IDesenvolvedoraUseCase desenvolvedoraUseCase)
        {
            _desenvolvedoraUseCase = desenvolvedoraUseCase;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista todas as desenvolvedoras",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista paginada de desenvolvedoras.
            * **Status 204 (No Content):** Executado com sucesso, porém não há desenvolvedoras cadastradas.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta.

            ## Observações:
            * Os dados incluem os **Jogos** relacionados a cada desenvolvedora.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem retornada com sucesso")]
        [SwaggerResponse(statusCode: 204, description: "Não há desenvolvedoras cadastradas")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados")]
        [EnableRateLimiting("politica_5_tentativas")]
        public async Task<IActionResult> Get(int Deslocamento = 0, int RegistroRetornado = 50)
        {
            try
            {
                var resultado = await _desenvolvedoraUseCase.ObterTodosDesenvolvedorasAsync(Deslocamento, RegistroRetornado);

                if (!resultado.Data.Any())
                    return NoContent();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
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

            ## Observações:
            * Os dados incluem os **Jogos** relacionados a essa desenvolvedora.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Desenvolvedora retornada com sucesso")]
        [SwaggerResponse(statusCode: 404, description: "Desenvolvedora não encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var desenvolvedora = await _desenvolvedoraUseCase.ObterUmaDesenvolvedoraAsync(id);

                if (desenvolvedora is null)
                    return NotFound();

                return Ok(desenvolvedora);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Adicionar desenvolvedora",
            Description = """
            ## Informações do Retorno:
            * **Status 201 (Created):** Desenvolvedora criada com sucesso.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao criar a desenvolvedora (ex: dados inválidos).
            """
        )]
        [SwaggerResponse(statusCode: 201, description: "Desenvolvedora criada com sucesso")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao criar a desenvolvedora")]
        public async Task<IActionResult> Post(DesenvolvedoraRequestDto model)
        {
            try
            {
                var desenvolvedora = await _desenvolvedoraUseCase.AdicionarDesenvolvedoraAsync(model);

                return CreatedAtAction(nameof(Get), new { id = desenvolvedora?.Id ?? 0 }, model);
            }
            catch (Exception ex)
            {
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
            * **Status 400 (Bad Request):** Ocorreu uma falha ao editar a desenvolvedora.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Desenvolvedora editada com sucesso")]
        [SwaggerResponse(statusCode: 404, description: "Desenvolvedora não encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao editar a desenvolvedora")]
        public async Task<IActionResult> Put(int id, DesenvolvedoraRequestDto model)
        {
            try
            {
                var desenvolvedora = await _desenvolvedoraUseCase.EditarDesenvolvedoraAsync(id, model);

                if (desenvolvedora is null)
                    return NotFound();

                return Ok(desenvolvedora);
            }
            catch (Exception ex)
            {
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
        [SwaggerResponse(statusCode: 200, description: "Desenvolvedora deletada com sucesso")]
        [SwaggerResponse(statusCode: 404, description: "Desenvolvedora não encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao deletar a desenvolvedora")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var desenvolvedora = await _desenvolvedoraUseCase.DeletarDesenvolvedoraAsync(id);

                if (desenvolvedora is null)
                    return NotFound();

                return Ok(desenvolvedora);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
