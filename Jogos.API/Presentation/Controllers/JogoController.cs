using Jogos.API.Application.Dtos;
using Jogos.API.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;

namespace Jogos.API.Presentation.Controllers
{
    [Route("api/jogo")]
    [ApiController]
    public class JogoController : ControllerBase
    {
        private readonly IJogoUseCase _jogoUseCase;

        public JogoController(IJogoUseCase jogoUseCase)
        {
            _jogoUseCase = jogoUseCase;
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
            * Os dados incluem as entidades relacionadas (**Desenvolvedora** e **Categorias**).
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem retornada com sucesso")]
        [SwaggerResponse(statusCode: 204, description: "Não há jogos cadastrados")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados")]
        [EnableRateLimiting("politica_5_tentativas")]
        public async Task<IActionResult> Get(int Deslocamento = 0, int RegistroRetornado = 50)
        {
            try
            {
                var resultado = await _jogoUseCase.ObterTodosJogosAsync(Deslocamento, RegistroRetornado);

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
            Summary = "Obter jogo por id",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna o jogo localizado.
            * **Status 404 (Not Found):** Não foi encontrado jogo com o id informado.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta.

            ## Observações:
            * Os dados incluem as entidades relacionadas (**Desenvolvedora** e **Categorias**).
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Jogo retornado com sucesso")]
        [SwaggerResponse(statusCode: 404, description: "Jogo não encontrado")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var jogo = await _jogoUseCase.ObterUmJogoAsync(id);

                if (jogo is null)
                    return NotFound();

                return Ok(jogo);
            }
            catch (Exception ex)
            {
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
            * Os dados incluem as entidades relacionadas (**Desenvolvedora** e **Categorias**).
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Jogos retornados com sucesso")]
        [SwaggerResponse(statusCode: 204, description: "Nenhum jogo encontrado com esse nome")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados")]
        [EnableRateLimiting("politica_5_tentativas")]
        public async Task<IActionResult> GetAllJogosByNome(string nome, int Deslocamento = 0, int RegistroRetornado = 50)
        {
            try
            {
                var resultado = await _jogoUseCase.ObterJogosPorNomeAsync(nome, Deslocamento, RegistroRetornado);

                if (!resultado.Any())
                    return NoContent();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("plataforma/{plataforma}")]
        [SwaggerOperation(
            Summary = "Lista jogos filtrando pela plataforma",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna os jogos disponíveis na plataforma informada.
            * **Status 204 (No Content):** Executado com sucesso, porém nenhum jogo encontrado nessa plataforma.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta.

            ## Observações:
            * Os dados incluem as entidades relacionadas (**Desenvolvedora** e **Categorias**).
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Jogos retornados com sucesso")]
        [SwaggerResponse(statusCode: 204, description: "Nenhum jogo encontrado nessa plataforma")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados")]
        [EnableRateLimiting("politica_5_tentativas")]
        public async Task<IActionResult> GetAllJogosByPlataforma(string plataforma, int Deslocamento = 0, int RegistroRetornado = 50)
        {
            try
            {
                var resultado = await _jogoUseCase.ObterJogosPorPlataformaAsync(plataforma, Deslocamento, RegistroRetornado);

                if (!resultado.Any())
                    return NoContent();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
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
            * Os dados incluem as entidades relacionadas (**Desenvolvedora** e **Categorias**).
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Jogos retornados com sucesso")]
        [SwaggerResponse(statusCode: 204, description: "Nenhum jogo encontrado para essa desenvolvedora")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados")]
        [EnableRateLimiting("politica_5_tentativas")]
        public async Task<IActionResult> GetAllJogosByDesenvolvedora(int idDesenvolvedora, int Deslocamento = 0, int RegistroRetornado = 50)
        {
            try
            {
                var resultado = await _jogoUseCase.ObterJogosPorDesenvolvedoraAsync(idDesenvolvedora, Deslocamento, RegistroRetornado);

                if (!resultado.Any())
                    return NoContent();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
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
            * Os dados incluem as entidades relacionadas (**Desenvolvedora** e **Categorias**).
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Jogos retornados com sucesso")]
        [SwaggerResponse(statusCode: 204, description: "Nenhum jogo encontrado para essa categoria")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados")]
        [EnableRateLimiting("politica_5_tentativas")]
        public async Task<IActionResult> GetAllJogosByCategoria(int idCategoria, int Deslocamento = 0, int RegistroRetornado = 50)
        {
            try
            {
                var resultado = await _jogoUseCase.ObterJogosPorCategoriaAsync(idCategoria, Deslocamento, RegistroRetornado);

                if (!resultado.Any())
                    return NoContent();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Adicionar jogo",
            Description = """
            ## Informações do Retorno:
            * **Status 201 (Created):** Jogo criado com sucesso.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao criar o jogo (ex: dados inválidos, desenvolvedora inexistente).

            ## Observações:
            * A vinculação com **Categorias** é feita separadamente pelo endpoint `POST /api/jogo/categoria/{idJogo}/{idCategoria}`.
            """
        )]
        [SwaggerResponse(statusCode: 201, description: "Jogo criado com sucesso")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao criar o jogo")]
        public async Task<IActionResult> Post(JogoRequestDto model)
        {
            try
            {
                var jogo = await _jogoUseCase.AdicionarJogoAsync(model);

                return CreatedAtAction(nameof(Get), new { id = jogo?.Id ?? 0 }, model);
            }
            catch (Exception ex)
            {
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
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Jogo editado com sucesso")]
        [SwaggerResponse(statusCode: 404, description: "Jogo não encontrado")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao editar o jogo")]
        public async Task<IActionResult> Put(int id, JogoRequestDto model)
        {
            try
            {
                var jogo = await _jogoUseCase.EditarJogoAsync(id, model);

                if (jogo is null)
                    return NotFound();

                return Ok(jogo);
            }
            catch (Exception ex)
            {
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
        [SwaggerResponse(statusCode: 200, description: "Jogo deletado com sucesso")]
        [SwaggerResponse(statusCode: 404, description: "Jogo não encontrado")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao deletar o jogo")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var jogo = await _jogoUseCase.DeletarJogoAsync(id);

                if (jogo is null)
                    return NotFound();

                return Ok(jogo);
            }
            catch (Exception ex)
            {
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
        [SwaggerResponse(statusCode: 200, description: "Categoria vinculada com sucesso")]
        [SwaggerResponse(statusCode: 404, description: "Jogo ou categoria não encontrados")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao vincular a categoria")]
        public async Task<IActionResult> PostCategoriaJogo(int idJogo, int idCategoria)
        {
            try
            {
                var jogo = await _jogoUseCase.VincularCategoriaAsync(idJogo, idCategoria);

                if (jogo is null)
                    return NotFound();

                return Ok(jogo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
