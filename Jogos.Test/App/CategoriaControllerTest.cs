using Jogos.API.Application.Dtos;
using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Models;
using Moq;
using System.Net;
using System.Net.Http.Json;

namespace Jogos.Test.App
{
    public class CategoriaControllerTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public CategoriaControllerTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        [Trait("Controller", "Categoria")]
        public async Task Get_SemCategorias_DeveRetornar204()
        {
            // Arrange
            _factory.CategoriaUseCaseMock
                .Setup(x => x.ObterTodosCategoriasAsync(0, 50))
                .ReturnsAsync(new PageResultModel<IEnumerable<CategoriaEntity>>
                {
                    Data = Enumerable.Empty<CategoriaEntity>(),
                    Deslocamento = 0,
                    RegistroRetornado = 50,
                    TotalRegistros = 0
                });

            using var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/categoria");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Categoria")]
        public async Task GetById_Inexistente_DeveRetornar404()
        {
            _factory.CategoriaUseCaseMock
                .Setup(x => x.ObterUmaCategoriaAsync(99999))
                .ReturnsAsync((CategoriaEntity?)null);

            using var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/categoria/99999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Categoria")]
        public async Task Post_DadosValidos_DeveRetornar201()
        {
            _factory.CategoriaUseCaseMock
                .Setup(x => x.AdicionarCategoriaAsync(It.IsAny<CategoriaRequestDto>()))
                .ReturnsAsync(new CategoriaEntity { Id = 1, Nome = "RPG" });

            using var client = _factory.CreateClient();

            var response = await client.PostAsJsonAsync("/api/categoria", new CategoriaRequestDto { Nome = "RPG" });

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Categoria")]
        public async Task Post_NomeDuplicado_DeveRetornar409()
        {
            _factory.CategoriaUseCaseMock
                .Setup(x => x.AdicionarCategoriaAsync(It.IsAny<CategoriaRequestDto>()))
                .ReturnsAsync((CategoriaEntity?)null);

            using var client = _factory.CreateClient();

            var response = await client.PostAsJsonAsync("/api/categoria", new CategoriaRequestDto { Nome = "RPG" });

            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Categoria")]
        public async Task Post_NomeInvalido_DeveRetornar400()
        {
            using var client = _factory.CreateClient();

            var response = await client.PostAsJsonAsync("/api/categoria", new CategoriaRequestDto { Nome = "A" });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Categoria")]
        public async Task Delete_Inexistente_DeveRetornar404()
        {
            _factory.CategoriaUseCaseMock
                .Setup(x => x.DeletarCategoriaAsync(99999))
                .ReturnsAsync((CategoriaEntity?)null);

            using var client = _factory.CreateClient();

            var response = await client.DeleteAsync("/api/categoria/99999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
