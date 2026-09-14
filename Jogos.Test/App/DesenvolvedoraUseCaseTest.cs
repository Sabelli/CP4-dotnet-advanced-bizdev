using Jogos.API.Application.Dtos;
using Jogos.API.Application.UseCases;
using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Interfaces;
using Jogos.API.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace Jogos.Test.App
{
    public class DesenvolvedoraUseCaseTest
    {
        private readonly Mock<IDesenvolvedoraRepository> _desenvolvedoraRepository;
        private readonly DesenvolvedoraUseCase _desenvolvedoraUseCase;

        public DesenvolvedoraUseCaseTest()
        {
            _desenvolvedoraRepository = new Mock<IDesenvolvedoraRepository>();
            _desenvolvedoraUseCase = new DesenvolvedoraUseCase(_desenvolvedoraRepository.Object, Mock.Of<ILogger<DesenvolvedoraUseCase>>());
        }

        [Fact]
        [Trait("UseCase", "Desenvolvedora")]
        public async Task ObterTodosDesenvolvedorasAsync_DeveRetornarDesenvolvedoras()
        {
            var pageResult = new PageResultModel<IEnumerable<DesenvolvedoraEntity>>
            {
                Data = new List<DesenvolvedoraEntity> { new DesenvolvedoraEntity { Id = 1, Nome = "CD Projekt Red" } },
                Deslocamento = 0,
                RegistroRetornado = 10,
                TotalRegistros = 1
            };

            _desenvolvedoraRepository.Setup(x => x.ObterTodosAsync(0, 10)).ReturnsAsync(pageResult);

            var resultado = await _desenvolvedoraUseCase.ObterTodosDesenvolvedorasAsync(0, 10);

            Assert.NotNull(resultado);
            Assert.Single(resultado.Data);
        }

        [Fact]
        [Trait("UseCase", "Desenvolvedora")]
        public async Task AdicionarDesenvolvedoraAsync_DeveChamarRepositoryComEntidadeMapeada()
        {
            var dto = new DesenvolvedoraRequestDto { Nome = "CD Projekt Red" };

            _desenvolvedoraRepository
                .Setup(x => x.AdicionarAsync(It.Is<DesenvolvedoraEntity>(e => e.Nome == "CD Projekt Red")))
                .ReturnsAsync((DesenvolvedoraEntity e) => e);

            var resultado = await _desenvolvedoraUseCase.AdicionarDesenvolvedoraAsync(dto);

            Assert.NotNull(resultado);
            Assert.Equal("CD Projekt Red", resultado!.Nome);
        }

        [Fact]
        [Trait("UseCase", "Desenvolvedora")]
        public async Task AdicionarDesenvolvedoraAsync_Duplicada_DeveRetornarNull()
        {
            _desenvolvedoraRepository.Setup(x => x.AdicionarAsync(It.IsAny<DesenvolvedoraEntity>())).ReturnsAsync((DesenvolvedoraEntity?)null);

            var resultado = await _desenvolvedoraUseCase.AdicionarDesenvolvedoraAsync(new DesenvolvedoraRequestDto { Nome = "CD Projekt Red" });

            Assert.Null(resultado);
        }

        [Fact]
        [Trait("UseCase", "Desenvolvedora")]
        public async Task ObterUmaDesenvolvedoraAsync_NaoEncontrada_DeveRetornarNull()
        {
            _desenvolvedoraRepository.Setup(x => x.ObterUmAsync(1)).ReturnsAsync((DesenvolvedoraEntity?)null);

            var resultado = await _desenvolvedoraUseCase.ObterUmaDesenvolvedoraAsync(1);

            Assert.Null(resultado);
        }
    }
}
