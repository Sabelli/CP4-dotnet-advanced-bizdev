using Jogos.API.Application.Dtos;
using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Exceptions;
using Jogos.API.Domain.Models;
using Moq;
using System.Net;
using System.Net.Http.Json;

namespace Jogos.Test.App
{
    public class DesenvolvedoraControllerTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public DesenvolvedoraControllerTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        [Trait("Controller", "Desenvolvedora")]
        public async Task Get_SemDesenvolvedoras_DeveRetornar204()
        {
            _factory.DesenvolvedoraUseCaseMock
                .Setup(x => x.ObterTodosDesenvolvedorasAsync(0, 50))
                .ReturnsAsync(new PageResultModel<IEnumerable<DesenvolvedoraEntity>>
                {
                    Data = Enumerable.Empty<DesenvolvedoraEntity>(),
                    Deslocamento = 0,
                    RegistroRetornado = 50,
                    TotalRegistros = 0
                });

            using var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/desenvolvedora");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Desenvolvedora")]
        public async Task GetById_Inexistente_DeveRetornar404()
        {
            _factory.DesenvolvedoraUseCaseMock
                .Setup(x => x.ObterUmaDesenvolvedoraAsync(99999))
                .ThrowsAsync(new EntidadeNaoEncontradaException("Desenvolvedora não encontrada para o id: 99999."));

            using var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/desenvolvedora/99999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Desenvolvedora")]
        public async Task Post_DadosValidos_DeveRetornar201()
        {
            _factory.DesenvolvedoraUseCaseMock
                .Setup(x => x.AdicionarDesenvolvedoraAsync(It.IsAny<DesenvolvedoraRequestDto>()))
                .ReturnsAsync(new DesenvolvedoraEntity { Id = 1, Nome = "CD Projekt Red" });

            using var client = _factory.CreateClient();

            var response = await client.PostAsJsonAsync("/api/desenvolvedora", new DesenvolvedoraRequestDto { Nome = "CD Projekt Red" });

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Desenvolvedora")]
        public async Task Post_NomeDuplicado_DeveRetornar409()
        {
            _factory.DesenvolvedoraUseCaseMock
                .Setup(x => x.AdicionarDesenvolvedoraAsync(It.IsAny<DesenvolvedoraRequestDto>()))
                .ReturnsAsync((DesenvolvedoraEntity?)null);

            using var client = _factory.CreateClient();

            var response = await client.PostAsJsonAsync("/api/desenvolvedora", new DesenvolvedoraRequestDto { Nome = "CD Projekt Red" });

            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Desenvolvedora")]
        public async Task Post_NomeInvalido_DeveRetornar400()
        {
            using var client = _factory.CreateClient();

            var response = await client.PostAsJsonAsync("/api/desenvolvedora", new DesenvolvedoraRequestDto { Nome = "A" });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
