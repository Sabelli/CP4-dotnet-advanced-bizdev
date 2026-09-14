using Jogos.API.Application.Dtos;
using Jogos.API.Application.UseCases;
using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Exceptions;
using Jogos.API.Domain.Interfaces;
using Jogos.API.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace Jogos.Test.App
{
    public class JogoUseCaseTest
    {
        private readonly Mock<IJogoRepository> _jogoRepository;
        private readonly JogoUseCase _jogoUseCase;

        public JogoUseCaseTest()
        {
            _jogoRepository = new Mock<IJogoRepository>();
            _jogoUseCase = new JogoUseCase(_jogoRepository.Object, Mock.Of<ILogger<JogoUseCase>>());
        }

        [Fact]
        [Trait("UseCase", "Jogo")]
        public async Task ObterTodosJogosAsync_DeveRetornarJogos()
        {
            var pageResult = new PageResultModel<IEnumerable<JogoEntity>>
            {
                Data = new List<JogoEntity> { new JogoEntity { Id = 1, Nome = "The Witcher 3" } },
                Deslocamento = 0,
                RegistroRetornado = 10,
                TotalRegistros = 1
            };

            _jogoRepository.Setup(x => x.ObterTodosAsync(0, 10)).ReturnsAsync(pageResult);

            var resultado = await _jogoUseCase.ObterTodosJogosAsync(0, 10);

            Assert.NotNull(resultado);
            Assert.Single(resultado.Data);
        }

        [Fact]
        [Trait("UseCase", "Jogo")]
        public async Task AdicionarJogoAsync_DeveRepassarCategoriaIdsEPlataformaIdsPraRepository()
        {
            // Arrange
            var dto = new JogoRequestDto
            {
                Nome = "The Witcher 3",
                Preco = 59.99,
                DataLancamento = new DateTime(2015, 5, 19),
                DesenvolvedoraId = 1,
                CategoriaIds = new List<int> { 1, 2 },
                PlataformaIds = new List<int> { 1 }
            };

            JogoEntity? entidadeAdicionada = null;
            IEnumerable<int>? categoriaIdsRecebidos = null;
            IEnumerable<int>? plataformaIdsRecebidos = null;

            _jogoRepository
                .Setup(x => x.AdicionarAsync(It.IsAny<JogoEntity>(), It.IsAny<IEnumerable<int>?>(), It.IsAny<IEnumerable<int>?>()))
                .Callback<JogoEntity, IEnumerable<int>?, IEnumerable<int>?>((entity, categoriaIds, plataformaIds) =>
                {
                    entidadeAdicionada = entity;
                    categoriaIdsRecebidos = categoriaIds;
                    plataformaIdsRecebidos = plataformaIds;
                })
                .ReturnsAsync((JogoEntity e, IEnumerable<int>? _, IEnumerable<int>? __) => e);

            // Act
            var resultado = await _jogoUseCase.AdicionarJogoAsync(dto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("The Witcher 3", entidadeAdicionada!.Nome);
            Assert.Equal(new List<int> { 1, 2 }, categoriaIdsRecebidos);
            Assert.Equal(new List<int> { 1 }, plataformaIdsRecebidos);
        }

        [Fact]
        [Trait("UseCase", "Jogo")]
        public async Task AdicionarJogoAsync_Duplicado_DeveRetornarNull()
        {
            _jogoRepository
                .Setup(x => x.AdicionarAsync(It.IsAny<JogoEntity>(), It.IsAny<IEnumerable<int>?>(), It.IsAny<IEnumerable<int>?>()))
                .ReturnsAsync((JogoEntity?)null);

            var resultado = await _jogoUseCase.AdicionarJogoAsync(new JogoRequestDto { Nome = "The Witcher 3", DesenvolvedoraId = 1 });

            Assert.Null(resultado);
        }

        [Fact]
        [Trait("UseCase", "Jogo")]
        public async Task VincularCategoriaAsync_DeveChamarRepository()
        {
            // Arrange
            var jogo = new JogoEntity { Id = 1, Nome = "The Witcher 3" };
            _jogoRepository.Setup(x => x.VincularCategoriaAsync(1, new[] { 2 })).ReturnsAsync(jogo);

            // Act
            var resultado = await _jogoUseCase.VincularCategoriaAsync(1, new[] { 2 });

            // Assert
            Assert.NotNull(resultado);
            _jogoRepository.Verify(x => x.VincularCategoriaAsync(1, new[] { 2 }), Times.Once);
        }

        [Fact]
        [Trait("UseCase", "Jogo")]
        public async Task VincularCategoriaAsync_CategoriaInexistente_DevePropagarExcecao()
        {
            // Arrange
            _jogoRepository
                .Setup(x => x.VincularCategoriaAsync(1, new[] { 99999 }))
                .ThrowsAsync(new EntidadeNaoEncontradaException("Categoria(s) não encontrada(s) para o(s) id(s): 99999."));

            // Act
            var excecao = await Record.ExceptionAsync(() => _jogoUseCase.VincularCategoriaAsync(1, new[] { 99999 }));

            // Assert
            Assert.IsType<EntidadeNaoEncontradaException>(excecao);
        }

        [Fact]
        [Trait("UseCase", "Jogo")]
        public async Task DesvincularPlataformaAsync_DeveChamarRepository()
        {
            // Arrange
            var jogo = new JogoEntity { Id = 1, Nome = "The Witcher 3" };
            _jogoRepository.Setup(x => x.DesvincularPlataformaAsync(1, new[] { 3 })).ReturnsAsync(jogo);

            // Act
            var resultado = await _jogoUseCase.DesvincularPlataformaAsync(1, new[] { 3 });

            // Assert
            Assert.NotNull(resultado);
            _jogoRepository.Verify(x => x.DesvincularPlataformaAsync(1, new[] { 3 }), Times.Once);
        }
    }
}
