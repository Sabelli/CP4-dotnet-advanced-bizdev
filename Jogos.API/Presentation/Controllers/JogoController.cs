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
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<JogoResponseDto>))]
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

                return Ok(resultado.ToResponseDto());
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
        [SwaggerResponse(statusCode: 200, description: "Jogo retornado com sucesso", type: typeof(JogoResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Jogo não encontrado", type: typeof(string))]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(JogoCreatedSample))]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("Obtendo jogo {JogoId}", id);

            try
            {
                var jogo = await _jogoUseCase.ObterUmJogoAsync(id);

                return Ok(jogo!.ToResponseDto());
            }
            catch (EntidadeNaoEncontradaException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return NotFound(ex.Message);
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
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<JogoResponseDto>))]
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

                if (!resultado.Data.Any())
                    return NoContent();

                return Ok(resultado.ToResponseDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar jogos pelo nome {Nome}", nome);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("plataforma/{idPlataforma}")]
        [SwaggerOperation(
            Summary = "Lista jogos filtrando pela plataforma",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna os jogos disponíveis na plataforma informada (busca pelo id da plataforma).
            * **Status 204 (No Content):** Executado com sucesso, porém nenhum jogo encontrado nessa plataforma.
            * **Status 404 (Not Found):** Plataforma informada não existe.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta.

            ## Observações:
            * Um jogo pode ter várias plataformas (relação **N:N**). Os dados incluem as entidades relacionadas (**Desenvolvedora**, **Categorias** e **Plataformas**).
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<JogoResponseDto>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum jogo encontrado nessa plataforma")]
        [SwaggerResponse(statusCode: 404, description: "Plataforma informada não existe", type: typeof(string))]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(JogoResponseListSample))]
        [EnableRateLimiting("politica_5_tentativas")]
        public async Task<IActionResult> GetAllJogosByPlataforma(int idPlataforma, int Deslocamento = 0, int RegistroRetornado = 50)
        {
            _logger.LogInformation("Listando jogos pela plataforma {PlataformaId}", idPlataforma);

            try
            {
                var resultado = await _jogoUseCase.ObterJogosPorPlataformaAsync(idPlataforma, Deslocamento, RegistroRetornado);

                if (!resultado.Data.Any())
                    return NoContent();

                return Ok(resultado.ToResponseDto());
            }
            catch (EntidadeNaoEncontradaException ex)
            {
                _logger.LogWarning(ex, "Plataforma {PlataformaId} não encontrada", idPlataforma);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar jogos pela plataforma {PlataformaId}", idPlataforma);
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
            * **Status 404 (Not Found):** Desenvolvedora informada não existe.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta.

            ## Observações:
            * Os dados incluem as entidades relacionadas (**Desenvolvedora**, **Categorias** e **Plataformas**).
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<JogoResponseDto>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum jogo encontrado para essa desenvolvedora")]
        [SwaggerResponse(statusCode: 404, description: "Desenvolvedora informada não existe", type: typeof(string))]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(JogoResponseListSample))]
        [EnableRateLimiting("politica_5_tentativas")]
        public async Task<IActionResult> GetAllJogosByDesenvolvedora(int idDesenvolvedora, int Deslocamento = 0, int RegistroRetornado = 50)
        {
            _logger.LogInformation("Listando jogos pela desenvolvedora {DesenvolvedoraId}", idDesenvolvedora);

            try
            {
                var resultado = await _jogoUseCase.ObterJogosPorDesenvolvedoraAsync(idDesenvolvedora, Deslocamento, RegistroRetornado);

                if (!resultado.Data.Any())
                    return NoContent();

                return Ok(resultado.ToResponseDto());
            }
            catch (EntidadeNaoEncontradaException ex)
            {
                _logger.LogWarning(ex, "Desenvolvedora {DesenvolvedoraId} não encontrada", idDesenvolvedora);
                return NotFound(ex.Message);
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
            * **Status 404 (Not Found):** Categoria informada não existe.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta.

            ## Observações:
            * Os dados incluem as entidades relacionadas (**Desenvolvedora**, **Categorias** e **Plataformas**).
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<JogoResponseDto>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum jogo encontrado para essa categoria")]
        [SwaggerResponse(statusCode: 404, description: "Categoria informada não existe", type: typeof(string))]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(JogoResponseListSample))]
        [EnableRateLimiting("politica_5_tentativas")]
        public async Task<IActionResult> GetAllJogosByCategoria(int idCategoria, int Deslocamento = 0, int RegistroRetornado = 50)
        {
            _logger.LogInformation("Listando jogos pela categoria {CategoriaId}", idCategoria);

            try
            {
                var resultado = await _jogoUseCase.ObterJogosPorCategoriaAsync(idCategoria, Deslocamento, RegistroRetornado);

                if (!resultado.Data.Any())
                    return NoContent();

                return Ok(resultado.ToResponseDto());
            }
            catch (EntidadeNaoEncontradaException ex)
            {
                _logger.LogWarning(ex, "Categoria {CategoriaId} não encontrada", idCategoria);
                return NotFound(ex.Message);
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
            * `CategoriaIds`/`PlataformaIds` são opcionais e vinculam categorias/plataformas já existentes no momento da criação. Pra vincular/desvincular depois, use os endpoints `POST`/`DELETE /api/jogo/categoria/{idJogo}` e `POST`/`DELETE /api/jogo/plataforma/{idJogo}` (corpo = lista de ids).
            """
        )]
        [SwaggerRequestExample(typeof(JogoRequestDto), typeof(JogoRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Jogo criado com sucesso", type: typeof(JogoResponseDto))]
        [SwaggerResponse(statusCode: 409, description: "Já existe um jogo com esse nome", type: typeof(string))]
        [SwaggerResponse(statusCode: 404, description: "Desenvolvedora não encontrada")]
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

                return CreatedAtAction(nameof(Get), new { id = jogo.Id }, jogo.ToResponseDto());
            }
            catch (EntidadeNaoEncontradaException ex)
            {
                _logger.LogWarning(ex, "Desenvolvedora não encontrada para o id: {DesenvolvedoraId}.", model.DesenvolvedoraId);
                return NotFound(ex.Message);
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
            * **Status 409 (Conflict):** Já existe um jogo com esse nome.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao editar o jogo.

            ## Observações:
            * Edição não mexe em categorias/plataformas — use os endpoints de vínculo/desvínculo pra alterar essas relações.
            """
        )]
        [SwaggerRequestExample(typeof(JogoUpdateRequestDto), typeof(JogoUpdateSample))]
        [SwaggerResponse(statusCode: 200, description: "Jogo editado com sucesso", type: typeof(JogoResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Jogo ou desenvolvedora não encontrado", type: typeof(string))]
        [SwaggerResponse(statusCode: 409, description: "Já existe um jogo com esse nome", type: typeof(string))]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao editar o jogo", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(JogoCreatedSample))]
        public async Task<IActionResult> Put(int id, JogoUpdateRequestDto model)
        {
            _logger.LogInformation("Editando jogo {JogoId}", id);

            try
            {
                var jogo = await _jogoUseCase.EditarJogoAsync(id, model);

                return Ok(jogo!.ToResponseDto());
            }
            catch (EntidadeNaoEncontradaException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return NotFound(ex.Message);
            }
            catch (NomeDuplicadoException ex)
            {
                _logger.LogWarning(ex, "Nome duplicado ao editar jogo {JogoId}", id);
                return Conflict(ex.Message);
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
        [SwaggerResponse(statusCode: 200, description: "Jogo deletado com sucesso", type: typeof(JogoResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Jogo não encontrado", type: typeof(string))]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao deletar o jogo", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(JogoCreatedSample))]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deletando jogo {JogoId}", id);

            try
            {
                var jogo = await _jogoUseCase.DeletarJogoAsync(id);

                return Ok(jogo!.ToResponseDto());
            }
            catch (EntidadeNaoEncontradaException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar jogo {JogoId}", id);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("categoria/{idJogo}")]
        [SwaggerOperation(
            Summary = "Vincular categorias existentes a um jogo",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Categorias vinculadas ao jogo com sucesso.
            * **Status 404 (Not Found):** Jogo não encontrado, ou algum id de categoria informado não existe.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao vincular as categorias.

            ## Observações:
            * Relação **N:N** entre Jogo e Categoria. Corpo da requisição é uma lista de ids de categoria.
            * Se algum id da lista não existir, nenhuma categoria é vinculada (falha a requisição inteira). Categorias já vinculadas são ignoradas (sem duplicar).
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Categorias vinculadas com sucesso", type: typeof(JogoResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Jogo não encontrado, ou algum id de categoria não existe", type: typeof(string))]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao vincular as categorias", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(JogoCreatedSample))]
        public async Task<IActionResult> PostCategoriaJogo(int idJogo, [FromBody] IEnumerable<int> categoriaIds)
        {
            _logger.LogInformation("Vinculando categorias {CategoriaIds} ao jogo {JogoId}", categoriaIds, idJogo);

            try
            {
                var jogo = await _jogoUseCase.VincularCategoriaAsync(idJogo, categoriaIds);

                return Ok(jogo!.ToResponseDto());
            }
            catch (EntidadeNaoEncontradaException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao vincular categorias {CategoriaIds} ao jogo {JogoId}", categoriaIds, idJogo);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("categoria/{idJogo}")]
        [SwaggerOperation(
            Summary = "Desvincular categorias de um jogo",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Categorias desvinculadas do jogo com sucesso.
            * **Status 404 (Not Found):** Jogo não encontrado.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao desvincular as categorias.

            ## Observações:
            * Corpo da requisição é uma lista de ids de categoria. Ids não vinculados ao jogo são ignorados.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Categorias desvinculadas com sucesso", type: typeof(JogoResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Jogo não encontrado", type: typeof(string))]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao desvincular as categorias", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(JogoCreatedSample))]
        public async Task<IActionResult> DeleteCategoriaJogo(int idJogo, [FromBody] IEnumerable<int> categoriaIds)
        {
            _logger.LogInformation("Desvinculando categorias {CategoriaIds} do jogo {JogoId}", categoriaIds, idJogo);

            try
            {
                var jogo = await _jogoUseCase.DesvincularCategoriaAsync(idJogo, categoriaIds);

                return Ok(jogo!.ToResponseDto());
            }
            catch (EntidadeNaoEncontradaException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao desvincular categorias {CategoriaIds} do jogo {JogoId}", categoriaIds, idJogo);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("plataforma/{idJogo}")]
        [SwaggerOperation(
            Summary = "Vincular plataformas existentes a um jogo",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Plataformas vinculadas ao jogo com sucesso.
            * **Status 404 (Not Found):** Jogo não encontrado, ou algum id de plataforma informado não existe.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao vincular as plataformas.

            ## Observações:
            * Relação **N:N** entre Jogo e Plataforma (um jogo pode estar em várias plataformas). Corpo da requisição é uma lista de ids de plataforma.
            * Se algum id da lista não existir, nenhuma plataforma é vinculada (falha a requisição inteira). Plataformas já vinculadas são ignoradas (sem duplicar).
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Plataformas vinculadas com sucesso", type: typeof(JogoResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Jogo não encontrado, ou algum id de plataforma não existe", type: typeof(string))]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao vincular as plataformas", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(JogoCreatedSample))]
        public async Task<IActionResult> PostPlataformaJogo(int idJogo, [FromBody] IEnumerable<int> plataformaIds)
        {
            _logger.LogInformation("Vinculando plataformas {PlataformaIds} ao jogo {JogoId}", plataformaIds, idJogo);

            try
            {
                var jogo = await _jogoUseCase.VincularPlataformaAsync(idJogo, plataformaIds);

                return Ok(jogo!.ToResponseDto());
            }
            catch (EntidadeNaoEncontradaException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao vincular plataformas {PlataformaIds} ao jogo {JogoId}", plataformaIds, idJogo);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("plataforma/{idJogo}")]
        [SwaggerOperation(
            Summary = "Desvincular plataformas de um jogo",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Plataformas desvinculadas do jogo com sucesso.
            * **Status 404 (Not Found):** Jogo não encontrado.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao desvincular as plataformas.

            ## Observações:
            * Corpo da requisição é uma lista de ids de plataforma. Ids não vinculados ao jogo são ignorados.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Plataformas desvinculadas com sucesso", type: typeof(JogoResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Jogo não encontrado", type: typeof(string))]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao desvincular as plataformas", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(JogoCreatedSample))]
        public async Task<IActionResult> DeletePlataformaJogo(int idJogo, [FromBody] IEnumerable<int> plataformaIds)
        {
            _logger.LogInformation("Desvinculando plataformas {PlataformaIds} do jogo {JogoId}", plataformaIds, idJogo);

            try
            {
                var jogo = await _jogoUseCase.DesvincularPlataformaAsync(idJogo, plataformaIds);

                return Ok(jogo!.ToResponseDto());
            }
            catch (EntidadeNaoEncontradaException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao desvincular plataformas {PlataformaIds} do jogo {JogoId}", plataformaIds, idJogo);
                return BadRequest(ex.Message);
            }
        }
    }
}
