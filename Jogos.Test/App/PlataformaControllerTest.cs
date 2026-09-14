using Jogos.API.Application.Dtos;
using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Models;
using Moq;
using System.Net;
using System.Net.Http.Json;

namespace Jogos.Test.App
{
    public class PlataformaControllerTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public PlataformaControllerTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact(DisplayName = "GET /api/plataforma sem registros retorna 204")]
        [Trait("Controller", "Plataforma")]
        public async Task Get_SemPlataformas_DeveRetornar204()
        {
            _factory.PlataformaUseCaseMock
                .Setup(x => x.ObterTodosPlataformasAsync(0, 50))
                .ReturnsAsync(new PageResultModel<IEnumerable<PlataformaEntity>>
                {
                    Data = Enumerable.Empty<PlataformaEntity>(),
                    Deslocamento = 0,
                    RegistroRetornado = 50,
                    TotalRegistros = 0
                });

            using var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/plataforma");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact(DisplayName = "POST /api/plataforma com nome de 2 caracteres retorna 201")]
        [Trait("Controller", "Plataforma")]
        public async Task Post_NomeDoisCaracteres_DeveRetornar201()
        {
            _factory.PlataformaUseCaseMock
                .Setup(x => x.AdicionarPlataformaAsync(It.IsAny<PlataformaRequestDto>()))
                .ReturnsAsync(new PlataformaEntity { Id = 1, Nome = "PC" });

            using var client = _factory.CreateClient();

            var response = await client.PostAsJsonAsync("/api/plataforma", new PlataformaRequestDto { Nome = "PC" });

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact(DisplayName = "POST /api/plataforma com nome de 1 caractere retorna 400")]
        [Trait("Controller", "Plataforma")]
        public async Task Post_NomeUmCaractere_DeveRetornar400()
        {
            using var client = _factory.CreateClient();

            var response = await client.PostAsJsonAsync("/api/plataforma", new PlataformaRequestDto { Nome = "P" });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact(DisplayName = "POST /api/plataforma com nome duplicado retorna 409")]
        [Trait("Controller", "Plataforma")]
        public async Task Post_NomeDuplicado_DeveRetornar409()
        {
            _factory.PlataformaUseCaseMock
                .Setup(x => x.AdicionarPlataformaAsync(It.IsAny<PlataformaRequestDto>()))
                .ReturnsAsync((PlataformaEntity?)null);

            using var client = _factory.CreateClient();

            var response = await client.PostAsJsonAsync("/api/plataforma", new PlataformaRequestDto { Nome = "PC" });

            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }
    }
}
