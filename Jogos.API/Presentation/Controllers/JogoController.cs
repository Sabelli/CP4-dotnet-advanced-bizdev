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
    [Route("api/jogo")]
    [ApiController]
    public class JogoController : ControllerBase
    {
        private readonly IJogoUseCase _jogoUseCase;
        private readonly ILogger<JogoController> _logger;

        public JogoController(IJogoUseCase jogoUseCase, ILogger<JogoController> logger)
        {
            _jogoUseCase = jogoUseCase;
            _logger = logger;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista todos os jogos",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista paginada de jogos.
            * **Status 204 (No Content):** Executado com sucesso, porém não há jogos cadastrados.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta.

            ## Observações:
            * Os dados incluem as entidades relacionadas (**Desenvolvedora**, **Categorias** e **Plataformas**).
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<JogoEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum jogo encontrado")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(JogoResponseListSample))]
        [EnableRateLimiting("politica_5_tentativas")]
        public async Task<IActionResult> Get(int Deslocamento = 0, int RegistroRetornado = 50)
        {
            _logger.LogInformation("Listando jogos, Deslocamento={Deslocamento}, RegistroRetornado={RegistroRetornado}", Deslocamento, RegistroRetornado);

            try
            {
                var resultado = await _jogoUseCase.ObterTodosJogosAsync(Deslocamento, RegistroRetornado);

                if (!resultado.Data.Any())
                    return NoContent();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar jogos");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Obter jogo por id",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna o jogo localizado.
            * **Status 404 (Not Found):** Não foi encontrado jogo com o id informado.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta.

            ## Observações:
            * Os dados incluem as entidades relacionadas (**Desenvolvedora**, **Categorias** e **Plataformas**).
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Jogo retornado com sucesso", type: typeof(JogoEntity))]
        [SwaggerResponse(statusCode: 404, description: "Jogo não encontrado")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(JogoResponseSample))]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("Obtendo jogo {JogoId}", id);

            try
            {
                var jogo = await _jogoUseCase.ObterUmJogoAsync(id);

                if (jogo is null)
                {
                    _logger.LogWarning("Jogo {JogoId} não encontrado", id);
                    return NotFound();
                }

                return Ok(jogo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter jogo {JogoId}", id);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("nome/{nome}")]
        [SwaggerOperation(
            Summary = "Lista jogos filtrando pelo nome",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna os jogos cujo nome contém o texto informado.
            * **Status 204 (No Content):** Executado com sucesso, porém nenhum jogo encontrado com esse nome.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta.

            ## Observações:
            * Os dados incluem as entidades relacionadas (**Desenvolvedora**, **Categorias** e **Plataformas**).
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<JogoEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum jogo encontrado com esse nome")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(JogoResponseListSample))]
        [EnableRateLimiting("politica_5_tentativas")]
        public async Task<IActionResult> GetAllJogosByNome(string nome, int Deslocamento = 0, int RegistroRetornado = 50)
        {
            _logger.LogInformation("Listando jogos pelo nome {Nome}", nome);

            try
            {
                var resultado = await _jogoUseCase.ObterJogosPorNomeAsync(nome, Deslocamento, RegistroRetornado);

                if (!resultado.Any())
                    return NoContent();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar jogos pelo nome {Nome}", nome);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("plataforma/{plataforma}")]
        [SwaggerOperation(
            Summary = "Lista jogos filtrando pela plataforma",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna os jogos disponíveis na plataforma informada (busca pelo nome da plataforma).
            * **Status 204 (No Content):** Executado com sucesso, porém nenhum jogo encontrado nessa plataforma.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta.

            ## Observações:
            * Um jogo pode ter várias plataformas (relação **N:N**). Os dados incluem as entidades relacionadas (**Desenvolvedora**, **Categorias** e **Plataformas**).
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<JogoEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum jogo encontrado nessa plataforma")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(JogoResponseListSample))]
        [EnableRateLimiting("politica_5_tentativas")]
        public async Task<IActionResult> GetAllJogosByPlataforma(string plataforma, int Deslocamento = 0, int RegistroRetornado = 50)
        {
            _logger.LogInformation("Listando jogos pela plataforma {Plataforma}", plataforma);

            try
            {
                var resultado = await _jogoUseCase.ObterJogosPorPlataformaAsync(plataforma, Deslocamento, RegistroRetornado);

                if (!resultado.Any())
                    return NoContent();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar jogos pela plataforma {Plataforma}", plataforma);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("desenvolvedora/{idDesenvolvedora}")]
        [SwaggerOperation(
            Summary = "Lista jogos filtrando pela desenvolvedora",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna os jogos da desenvolvedora informada.
            * **Status 204 (No Content):** Executado com sucesso, porém nenhum jogo encontrado para essa desenvolvedora.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta.

            ## Observações:
            * Os dados incluem as entidades relacionadas (**Desenvolvedora**, **Categorias** e **Plataformas**).
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<JogoEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum jogo encontrado para essa desenvolvedora")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(JogoResponseListSample))]
        [EnableRateLimiting("politica_5_tentativas")]
        public async Task<IActionResult> GetAllJogosByDesenvolvedora(int idDesenvolvedora, int Deslocamento = 0, int RegistroRetornado = 50)
        {
            _logger.LogInformation("Listando jogos pela desenvolvedora {DesenvolvedoraId}", idDesenvolvedora);

            try
            {
                var resultado = await _jogoUseCase.ObterJogosPorDesenvolvedoraAsync(idDesenvolvedora, Deslocamento, RegistroRetornado);

                if (!resultado.Any())
                    return NoContent();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar jogos pela desenvolvedora {DesenvolvedoraId}", idDesenvolvedora);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("categoria/{idCategoria}")]
        [SwaggerOperation(
            Summary = "Lista jogos filtrando pela categoria",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna os jogos vinculados à categoria informada.
            * **Status 204 (No Content):** Executado com sucesso, porém nenhum jogo encontrado para essa categoria.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta.

            ## Observações:
            * Os dados incluem as entidades relacionadas (**Desenvolvedora**, **Categorias** e **Plataformas**).
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<JogoEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum jogo encontrado para essa categoria")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(JogoResponseListSample))]
        [EnableRateLimiting("politica_5_tentativas")]
        public async Task<IActionResult> GetAllJogosByCategoria(int idCategoria, int Deslocamento = 0, int RegistroRetornado = 50)
        {
            _logger.LogInformation("Listando jogos pela categoria {CategoriaId}", idCategoria);

            try
            {
                var resultado = await _jogoUseCase.ObterJogosPorCategoriaAsync(idCategoria, Deslocamento, RegistroRetornado);

                if (!resultado.Any())
                    return NoContent();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar jogos pela categoria {CategoriaId}", idCategoria);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Adicionar jogo",
            Description = """
            ## Informações do Retorno:
            * **Status 201 (Created):** Jogo criado com sucesso.
            * **Status 409 (Conflict):** Já existe um jogo com esse nome.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao criar o jogo (ex: dados inválidos, desenvolvedora inexistente).

            ## Observações:
            * `CategoriaIds`/`PlataformaIds` são opcionais e vinculam categorias/plataformas já existentes no momento da criação. Pra vincular/desvincular depois, use os endpoints `POST`/`DELETE /api/jogo/categoria/{idJogo}/{idCategoria}` e `POST`/`DELETE /api/jogo/plataforma/{idJogo}/{idPlataforma}`.
            """
        )]
        [SwaggerRequestExample(typeof(JogoRequestDto), typeof(JogoRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Jogo criado com sucesso", type: typeof(JogoEntity))]
        [SwaggerResponse(statusCode: 409, description: "Já existe um jogo com esse nome", type: typeof(string))]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao criar o jogo", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 201, typeof(JogoCreatedSample))]
        public async Task<IActionResult> Post(JogoRequestDto model)
        {
            _logger.LogInformation("Criando jogo {Nome}", model.Nome);

            try
            {
                var jogo = await _jogoUseCase.AdicionarJogoAsync(model);

                if (jogo is null)
                {
                    _logger.LogWarning("Jogo {Nome} já existe", model.Nome);
                    return Conflict($"Já existe um jogo com o nome '{model.Nome}'.");
                }

                return CreatedAtAction(nameof(Get), new { id = jogo.Id }, jogo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar jogo {Nome}", model.Nome);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Editar jogo",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Jogo editado com sucesso.
            * **Status 404 (Not Found):** Não foi encontrado jogo com o id informado.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao editar o jogo.

            ## Observações:
            * Edição não mexe em categorias/plataformas — use os endpoints de vínculo/desvínculo pra alterar essas relações.
            """
        )]
        [SwaggerRequestExample(typeof(JogoUpdateRequestDto), typeof(JogoUpdateSample))]
        [SwaggerResponse(statusCode: 200, description: "Jogo editado com sucesso", type: typeof(JogoEntity))]
        [SwaggerResponse(statusCode: 404, description: "Jogo não encontrado")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao editar o jogo", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(JogoResponseSample))]
        public async Task<IActionResult> Put(int id, JogoUpdateRequestDto model)
        {
            _logger.LogInformation("Editando jogo {JogoId}", id);

            try
            {
                var jogo = await _jogoUseCase.EditarJogoAsync(id, model);

                if (jogo is null)
                {
                    _logger.LogWarning("Jogo {JogoId} não encontrado para edição", id);
                    return NotFound();
                }

                return Ok(jogo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao editar jogo {JogoId}", id);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Deletar jogo",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Jogo deletado com sucesso.
            * **Status 404 (Not Found):** Não foi encontrado jogo com o id informado.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao deletar o jogo.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Jogo deletado com sucesso", type: typeof(JogoEntity))]
        [SwaggerResponse(statusCode: 404, description: "Jogo não encontrado")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao deletar o jogo", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(JogoResponseSample))]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deletando jogo {JogoId}", id);

            try
            {
                var jogo = await _jogoUseCase.DeletarJogoAsync(id);

                if (jogo is null)
                {
                    _logger.LogWarning("Jogo {JogoId} não encontrado para deleção", id);
                    return NotFound();
                }

                return Ok(jogo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar jogo {JogoId}", id);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("categoria/{idJogo}/{idCategoria}")]
        [SwaggerOperation(
            Summary = "Vincular categoria existente a um jogo",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Categoria vinculada ao jogo com sucesso.
            * **Status 404 (Not Found):** Jogo ou categoria não encontrados.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao vincular a categoria.

            ## Observações:
            * Relação **N:N** entre Jogo e Categoria. Se a categoria já estiver vinculada, nada é alterado.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Categoria vinculada com sucesso", type: typeof(JogoEntity))]
        [SwaggerResponse(statusCode: 404, description: "Jogo ou categoria não encontrados")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao vincular a categoria", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(JogoResponseSample))]
        public async Task<IActionResult> PostCategoriaJogo(int idJogo, int idCategoria)
        {
            _logger.LogInformation("Vinculando categoria {CategoriaId} ao jogo {JogoId}", idCategoria, idJogo);

            try
            {
                var jogo = await _jogoUseCase.VincularCategoriaAsync(idJogo, idCategoria);

                if (jogo is null)
                {
                    _logger.LogWarning("Jogo {JogoId} ou categoria {CategoriaId} não encontrados para vínculo", idJogo, idCategoria);
                    return NotFound();
                }

                return Ok(jogo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao vincular categoria {CategoriaId} ao jogo {JogoId}", idCategoria, idJogo);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("categoria/{idJogo}/{idCategoria}")]
        [SwaggerOperation(
            Summary = "Desvincular categoria de um jogo",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Categoria desvinculada do jogo com sucesso.
            * **Status 404 (Not Found):** Jogo não encontrado.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao desvincular a categoria.

            ## Observações:
            * Se a categoria não estiver vinculada, nada é alterado.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Categoria desvinculada com sucesso", type: typeof(JogoEntity))]
        [SwaggerResponse(statusCode: 404, description: "Jogo não encontrado")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao desvincular a categoria", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(JogoResponseSample))]
        public async Task<IActionResult> DeleteCategoriaJogo(int idJogo, int idCategoria)
        {
            _logger.LogInformation("Desvinculando categoria {CategoriaId} do jogo {JogoId}", idCategoria, idJogo);

            try
            {
                var jogo = await _jogoUseCase.DesvincularCategoriaAsync(idJogo, idCategoria);

                if (jogo is null)
                {
                    _logger.LogWarning("Jogo {JogoId} não encontrado para desvínculo", idJogo);
                    return NotFound();
                }

                return Ok(jogo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao desvincular categoria {CategoriaId} do jogo {JogoId}", idCategoria, idJogo);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("plataforma/{idJogo}/{idPlataforma}")]
        [SwaggerOperation(
            Summary = "Vincular plataforma existente a um jogo",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Plataforma vinculada ao jogo com sucesso.
            * **Status 404 (Not Found):** Jogo ou plataforma não encontrados.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao vincular a plataforma.

            ## Observações:
            * Relação **N:N** entre Jogo e Plataforma (um jogo pode estar em várias plataformas). Se a plataforma já estiver vinculada, nada é alterado.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Plataforma vinculada com sucesso", type: typeof(JogoEntity))]
        [SwaggerResponse(statusCode: 404, description: "Jogo ou plataforma não encontrados")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao vincular a plataforma", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(JogoResponseSample))]
        public async Task<IActionResult> PostPlataformaJogo(int idJogo, int idPlataforma)
        {
            _logger.LogInformation("Vinculando plataforma {PlataformaId} ao jogo {JogoId}", idPlataforma, idJogo);

            try
            {
                var jogo = await _jogoUseCase.VincularPlataformaAsync(idJogo, idPlataforma);

                if (jogo is null)
                {
                    _logger.LogWarning("Jogo {JogoId} ou plataforma {PlataformaId} não encontrados para vínculo", idJogo, idPlataforma);
                    return NotFound();
                }

                return Ok(jogo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao vincular plataforma {PlataformaId} ao jogo {JogoId}", idPlataforma, idJogo);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("plataforma/{idJogo}/{idPlataforma}")]
        [SwaggerOperation(
            Summary = "Desvincular plataforma de um jogo",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Plataforma desvinculada do jogo com sucesso.
            * **Status 404 (Not Found):** Jogo não encontrado.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao desvincular a plataforma.

            ## Observações:
            * Se a plataforma não estiver vinculada, nada é alterado.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Plataforma desvinculada com sucesso", type: typeof(JogoEntity))]
        [SwaggerResponse(statusCode: 404, description: "Jogo não encontrado")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao desvincular a plataforma", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(JogoResponseSample))]
        public async Task<IActionResult> DeletePlataformaJogo(int idJogo, int idPlataforma)
        {
            _logger.LogInformation("Desvinculando plataforma {PlataformaId} do jogo {JogoId}", idPlataforma, idJogo);

            try
            {
                var jogo = await _jogoUseCase.DesvincularPlataformaAsync(idJogo, idPlataforma);

                if (jogo is null)
                {
                    _logger.LogWarning("Jogo {JogoId} não encontrado para desvínculo", idJogo);
                    return NotFound();
                }

                return Ok(jogo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao desvincular plataforma {PlataformaId} do jogo {JogoId}", idPlataforma, idJogo);
                return BadRequest(ex.Message);
            }
        }
    }
}
