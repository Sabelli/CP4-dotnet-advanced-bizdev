using Jogos.API.Application.Dtos;
using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Exceptions;
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

        [Fact]
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

        [Fact]
        [Trait("Controller", "Jogo")]
        public async Task GetById_Inexistente_DeveRetornar404()
        {
            _factory.JogoUseCaseMock
                .Setup(x => x.ObterUmJogoAsync(99999))
                .ThrowsAsync(new EntidadeNaoEncontradaException("Jogo não encontrado para o id: 99999."));

            using var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/jogo/99999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
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

        [Fact]
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

        [Fact]
        [Trait("Controller", "Jogo")]
        public async Task PostCategoriaJogo_JogoECategoriaExistentes_DeveRetornar200()
        {
            // Arrange
            _factory.JogoUseCaseMock
                .Setup(x => x.VincularCategoriaAsync(1, new[] { 2 }))
                .ReturnsAsync(new JogoEntity { Id = 1, Nome = "The Witcher 3" });

            using var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("/api/jogo/categoria/1", new[] { 2 });

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Jogo")]
        public async Task PostCategoriaJogo_JogoInexistente_DeveRetornar404()
        {
            // Arrange
            _factory.JogoUseCaseMock
                .Setup(x => x.VincularCategoriaAsync(99999, new[] { 2 }))
                .ThrowsAsync(new EntidadeNaoEncontradaException("Jogo não encontrado para o id: 99999."));

            using var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("/api/jogo/categoria/99999", new[] { 2 });

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Jogo")]
        public async Task PostCategoriaJogo_CategoriaInexistente_DeveRetornar404()
        {
            // Arrange
            _factory.JogoUseCaseMock
                .Setup(x => x.VincularCategoriaAsync(1, new[] { 99999 }))
                .ThrowsAsync(new EntidadeNaoEncontradaException("Categoria(s) não encontrada(s) para o(s) id(s): 99999."));

            using var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("/api/jogo/categoria/1", new[] { 99999 });

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Jogo")]
        public async Task DeletePlataformaJogo_VinculoExistente_DeveRetornar200()
        {
            // Arrange
            _factory.JogoUseCaseMock
                .Setup(x => x.DesvincularPlataformaAsync(1, new[] { 1 }))
                .ReturnsAsync(new JogoEntity { Id = 1, Nome = "The Witcher 3" });

            using var client = _factory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Delete, "/api/jogo/plataforma/1")
            {
                Content = JsonContent.Create(new[] { 1 })
            };

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
