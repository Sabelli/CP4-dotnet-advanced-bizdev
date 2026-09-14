using Jogos.API.Application.Dtos;
using Jogos.API.Application.UseCases;
using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Interfaces;
using Jogos.API.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace Jogos.Test.App
{
    public class CategoriaUseCaseTest
    {
        private readonly Mock<ICategoriaRepository> _categoriaRepository;
        private readonly CategoriaUseCase _categoriaUseCase;

        public CategoriaUseCaseTest()
        {
            _categoriaRepository = new Mock<ICategoriaRepository>();
            _categoriaUseCase = new CategoriaUseCase(_categoriaRepository.Object, Mock.Of<ILogger<CategoriaUseCase>>());
        }

        [Fact]
        [Trait("UseCase", "Categoria")]
        public async Task ObterTodosCategoriasAsync_DeveRetornarCategorias()
        {
            // Arrange
            var pageResult = new PageResultModel<IEnumerable<CategoriaEntity>>
            {
                Data = new List<CategoriaEntity> { new CategoriaEntity { Id = 1, Nome = "RPG" } },
                Deslocamento = 0,
                RegistroRetornado = 10,
                TotalRegistros = 1
            };

            _categoriaRepository.Setup(x => x.ObterTodosAsync(0, 10)).ReturnsAsync(pageResult);

            // Act
            var resultado = await _categoriaUseCase.ObterTodosCategoriasAsync(0, 10);

            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado.Data);
        }

        [Fact]
        [Trait("UseCase", "Categoria")]
        public async Task ObterUmaCategoriaAsync_DeveRetornarCategoria()
        {
            var categoria = new CategoriaEntity { Id = 1, Nome = "RPG" };
            _categoriaRepository.Setup(x => x.ObterUmAsync(1)).ReturnsAsync(categoria);

            var resultado = await _categoriaUseCase.ObterUmaCategoriaAsync(1);

            Assert.NotNull(resultado);
            Assert.Equal("RPG", resultado!.Nome);
        }

        [Fact]
        [Trait("UseCase", "Categoria")]
        public async Task AdicionarCategoriaAsync_DeveChamarRepositoryComEntidadeMapeada()
        {
            // Arrange
            var dto = new CategoriaRequestDto { Nome = "RPG" };

            _categoriaRepository
                .Setup(x => x.AdicionarAsync(It.Is<CategoriaEntity>(e => e.Nome == "RPG")))
                .ReturnsAsync((CategoriaEntity e) => e);

            // Act
            var resultado = await _categoriaUseCase.AdicionarCategoriaAsync(dto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("RPG", resultado!.Nome);
        }

        [Fact]
        [Trait("UseCase", "Categoria")]
        public async Task AdicionarCategoriaAsync_Duplicada_DeveRetornarNull()
        {
            _categoriaRepository.Setup(x => x.AdicionarAsync(It.IsAny<CategoriaEntity>())).ReturnsAsync((CategoriaEntity?)null);

            var resultado = await _categoriaUseCase.AdicionarCategoriaAsync(new CategoriaRequestDto { Nome = "RPG" });

            Assert.Null(resultado);
        }

        [Fact]
        [Trait("UseCase", "Categoria")]
        public async Task DeletarCategoriaAsync_NaoEncontrada_DeveRetornarNull()
        {
            _categoriaRepository.Setup(x => x.DeletarAsync(1)).ReturnsAsync((CategoriaEntity?)null);

            var resultado = await _categoriaUseCase.DeletarCategoriaAsync(1);

            Assert.Null(resultado);
        }
    }
}
