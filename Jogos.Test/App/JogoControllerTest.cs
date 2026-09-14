using Jogos.API.Application.Dtos;
using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Models;
using Moq;
using System.Net;
using System.Net.Http.Json;

namespace Jogos.Test.App
{
    public class JogoControllerTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public JogoControllerTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact(DisplayName = "GET /api/jogo sem registros retorna 204")]
        [Trait("Controller", "Jogo")]
        public async Task Get_SemJogos_DeveRetornar204()
        {
            _factory.JogoUseCaseMock
                .Setup(x => x.ObterTodosJogosAsync(0, 50))
                .ReturnsAsync(new PageResultModel<IEnumerable<JogoEntity>>
                {
                    Data = Enumerable.Empty<JogoEntity>(),
                    Deslocamento = 0,
                    RegistroRetornado = 50,
                    TotalRegistros = 0
                });

            using var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/jogo");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact(DisplayName = "GET /api/jogo/{id} inexistente retorna 404")]
        [Trait("Controller", "Jogo")]
        public async Task GetById_Inexistente_DeveRetornar404()
        {
            _factory.JogoUseCaseMock
                .Setup(x => x.ObterUmJogoAsync(99999))
                .ReturnsAsync((JogoEntity?)null);

            using var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/jogo/99999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact(DisplayName = "POST /api/jogo com dados válidos retorna 201")]
        [Trait("Controller", "Jogo")]
        public async Task Post_DadosValidos_DeveRetornar201()
        {
            _factory.JogoUseCaseMock
                .Setup(x => x.AdicionarJogoAsync(It.IsAny<JogoRequestDto>()))
                .ReturnsAsync(new JogoEntity { Id = 1, Nome = "The Witcher 3" });

            using var client = _factory.CreateClient();

            var response = await client.PostAsJsonAsync("/api/jogo", new JogoRequestDto
            {
                Nome = "The Witcher 3",
                Preco = 59.99,
                DataLancamento = new DateTime(2015, 5, 19),
                DesenvolvedoraId = 1
            });

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact(DisplayName = "POST /api/jogo com nome duplicado retorna 409")]
        [Trait("Controller", "Jogo")]
        public async Task Post_NomeDuplicado_DeveRetornar409()
        {
            _factory.JogoUseCaseMock
                .Setup(x => x.AdicionarJogoAsync(It.IsAny<JogoRequestDto>()))
                .ReturnsAsync((JogoEntity?)null);

            using var client = _factory.CreateClient();

            var response = await client.PostAsJsonAsync("/api/jogo", new JogoRequestDto
            {
                Nome = "The Witcher 3",
                Preco = 59.99,
                DataLancamento = new DateTime(2015, 5, 19),
                DesenvolvedoraId = 1
            });

            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact(DisplayName = "POST /api/jogo/categoria/{idJogo}/{idCategoria} vínculo existente retorna 200")]
        [Trait("Controller", "Jogo")]
        public async Task PostCategoriaJogo_JogoECategoriaExistentes_DeveRetornar200()
        {
            _factory.JogoUseCaseMock
                .Setup(x => x.VincularCategoriaAsync(1, 2))
                .ReturnsAsync(new JogoEntity { Id = 1, Nome = "The Witcher 3" });

            using var client = _factory.CreateClient();

            var response = await client.PostAsync("/api/jogo/categoria/1/2", null);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "POST /api/jogo/categoria/{idJogo}/{idCategoria} jogo inexistente retorna 404")]
        [Trait("Controller", "Jogo")]
        public async Task PostCategoriaJogo_JogoInexistente_DeveRetornar404()
        {
            _factory.JogoUseCaseMock
                .Setup(x => x.VincularCategoriaAsync(99999, 2))
                .ReturnsAsync((JogoEntity?)null);

            using var client = _factory.CreateClient();

            var response = await client.PostAsync("/api/jogo/categoria/99999/2", null);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact(DisplayName = "DELETE /api/jogo/plataforma/{idJogo}/{idPlataforma} desvincula com 200")]
        [Trait("Controller", "Jogo")]
        public async Task DeletePlataformaJogo_VinculoExistente_DeveRetornar200()
        {
            _factory.JogoUseCaseMock
                .Setup(x => x.DesvincularPlataformaAsync(1, 1))
                .ReturnsAsync(new JogoEntity { Id = 1, Nome = "The Witcher 3" });

            using var client = _factory.CreateClient();

            var response = await client.DeleteAsync("/api/jogo/plataforma/1/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
