using Jogos.API.Application.Dtos;
using Jogos.API.Application.Interfaces;
using Jogos.API.Doc.Samples;
using Jogos.API.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Jogos.API.Presentation.Controllers
{
    [Route("api/categoria")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaUseCase _categoriaUseCase;
        private readonly ILogger<CategoriaController> _logger;

        public CategoriaController(ICategoriaUseCase categoriaUseCase, ILogger<CategoriaController> logger)
        {
            _categoriaUseCase = categoriaUseCase;
            _logger = logger;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista todas as categorias",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista paginada de categorias.
            * **Status 204 (No Content):** Executado com sucesso, porém não há categorias cadastradas.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta.

            ## Observações:
            * Os dados incluem os **Jogos** relacionados a cada categoria.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<CategoriaEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma categoria encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(CategoriaResponseListSample))]
        [EnableRateLimiting("politica_5_tentativas")]
        public async Task<IActionResult> Get(int Deslocamento = 0, int RegistroRetornado = 50)
        {
            _logger.LogInformation("Listando categorias, Deslocamento={Deslocamento}, RegistroRetornado={RegistroRetornado}", Deslocamento, RegistroRetornado);

            try
            {
                var resultado = await _categoriaUseCase.ObterTodosCategoriasAsync(Deslocamento, RegistroRetornado);

                if (!resultado.Data.Any())
                    return NoContent();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar categorias");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Obter categoria por id",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna a categoria localizada.
            * **Status 404 (Not Found):** Não foi encontrada categoria com o id informado.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta.

            ## Observações:
            * Os dados incluem os **Jogos** relacionados a essa categoria.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Categoria retornada com sucesso", type: typeof(CategoriaEntity))]
        [SwaggerResponse(statusCode: 404, description: "Categoria não encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(CategoriaResponseSample))]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("Obtendo categoria {CategoriaId}", id);

            try
            {
                var categoria = await _categoriaUseCase.ObterUmaCategoriaAsync(id);

                if (categoria is null)
                {
                    _logger.LogWarning("Categoria {CategoriaId} não encontrada", id);
                    return NotFound();
                }

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter categoria {CategoriaId}", id);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Adicionar categoria",
            Description = """
            ## Informações do Retorno:
            * **Status 201 (Created):** Categoria criada com sucesso.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao criar a categoria (ex: dados inválidos).
            """
        )]
        [SwaggerRequestExample(typeof(CategoriaRequestDto), typeof(CategoriaRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Categoria criada com sucesso", type: typeof(CategoriaEntity))]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao criar a categoria", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 201, typeof(CategoriaResponseSample))]
        public async Task<IActionResult> Post(CategoriaRequestDto model)
        {
            _logger.LogInformation("Criando categoria {Nome}", model.Nome);

            try
            {
                var categoria = await _categoriaUseCase.AdicionarCategoriaAsync(model);

                return CreatedAtAction(nameof(Get), new { id = categoria?.Id ?? 0 }, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar categoria {Nome}", model.Nome);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Editar categoria",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Categoria editada com sucesso.
            * **Status 404 (Not Found):** Não foi encontrada categoria com o id informado.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao editar a categoria.
            """
        )]
        [SwaggerRequestExample(typeof(CategoriaRequestDto), typeof(CategoriaRequestSample))]
        [SwaggerResponse(statusCode: 200, description: "Categoria editada com sucesso", type: typeof(CategoriaEntity))]
        [SwaggerResponse(statusCode: 404, description: "Categoria não encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao editar a categoria", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(CategoriaResponseSample))]
        public async Task<IActionResult> Put(int id, CategoriaRequestDto model)
        {
            _logger.LogInformation("Editando categoria {CategoriaId}", id);

            try
            {
                var categoria = await _categoriaUseCase.EditarCategoriaAsync(id, model);

                if (categoria is null)
                {
                    _logger.LogWarning("Categoria {CategoriaId} não encontrada para edição", id);
                    return NotFound();
                }

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao editar categoria {CategoriaId}", id);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Deletar categoria",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Categoria deletada com sucesso.
            * **Status 404 (Not Found):** Não foi encontrada categoria com o id informado.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao deletar a categoria.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Categoria deletada com sucesso", type: typeof(CategoriaEntity))]
        [SwaggerResponse(statusCode: 404, description: "Categoria não encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao deletar a categoria", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(CategoriaResponseSample))]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deletando categoria {CategoriaId}", id);

            try
            {
                var categoria = await _categoriaUseCase.DeletarCategoriaAsync(id);

                if (categoria is null)
                {
                    _logger.LogWarning("Categoria {CategoriaId} não encontrada para deleção", id);
                    return NotFound();
                }

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar categoria {CategoriaId}", id);
                return BadRequest(ex.Message);
            }
        }
    }
}
