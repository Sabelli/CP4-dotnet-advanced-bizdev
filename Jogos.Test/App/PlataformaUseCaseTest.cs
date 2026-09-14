using Jogos.API.Application.Dtos;
using Jogos.API.Application.UseCases;
using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Interfaces;
using Jogos.API.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace Jogos.Test.App
{
    public class PlataformaUseCaseTest
    {
        private readonly Mock<IPlataformaRepository> _plataformaRepository;
        private readonly PlataformaUseCase _plataformaUseCase;

        public PlataformaUseCaseTest()
        {
            _plataformaRepository = new Mock<IPlataformaRepository>();
            _plataformaUseCase = new PlataformaUseCase(_plataformaRepository.Object, Mock.Of<ILogger<PlataformaUseCase>>());
        }

        [Fact]
        [Trait("UseCase", "Plataforma")]
        public async Task ObterTodosPlataformasAsync_DeveRetornarPlataformas()
        {
            var pageResult = new PageResultModel<IEnumerable<PlataformaEntity>>
            {
                Data = new List<PlataformaEntity> { new PlataformaEntity { Id = 1, Nome = "PC" } },
                Deslocamento = 0,
                RegistroRetornado = 10,
                TotalRegistros = 1
            };

            _plataformaRepository.Setup(x => x.ObterTodosAsync(0, 10)).ReturnsAsync(pageResult);

            var resultado = await _plataformaUseCase.ObterTodosPlataformasAsync(0, 10);

            Assert.NotNull(resultado);
            Assert.Single(resultado.Data);
        }

        [Fact]
        [Trait("UseCase", "Plataforma")]
        public async Task AdicionarPlataformaAsync_DeveChamarRepositoryComEntidadeMapeada()
        {
            var dto = new PlataformaRequestDto { Nome = "PC" };

            _plataformaRepository
                .Setup(x => x.AdicionarAsync(It.Is<PlataformaEntity>(e => e.Nome == "PC")))
                .ReturnsAsync((PlataformaEntity e) => e);

            var resultado = await _plataformaUseCase.AdicionarPlataformaAsync(dto);

            Assert.NotNull(resultado);
            Assert.Equal("PC", resultado!.Nome);
        }

        [Fact]
        [Trait("UseCase", "Plataforma")]
        public async Task AdicionarPlataformaAsync_Duplicada_DeveRetornarNull()
        {
            _plataformaRepository.Setup(x => x.AdicionarAsync(It.IsAny<PlataformaEntity>())).ReturnsAsync((PlataformaEntity?)null);

            var resultado = await _plataformaUseCase.AdicionarPlataformaAsync(new PlataformaRequestDto { Nome = "PC" });

            Assert.Null(resultado);
        }

        [Fact]
        [Trait("UseCase", "Plataforma")]
        public async Task ObterUmaPlataformaAsync_NaoEncontrada_DeveRetornarNull()
        {
            _plataformaRepository.Setup(x => x.ObterUmAsync(1)).ReturnsAsync((PlataformaEntity?)null);

            var resultado = await _plataformaUseCase.ObterUmaPlataformaAsync(1);

            Assert.Null(resultado);
        }
    }
}
