using Jogos.API.Domain.Entities;
using Jogos.API.Infrastructure.Data;
using Jogos.API.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Jogos.Test.App
{
    public class CategoriaRepositoryTest
    {
        private readonly ApplicationContext _applicationContext;
        private readonly CategoriaRepository _categoriaRepository;

        public CategoriaRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _applicationContext = new ApplicationContext(options);
            _applicationContext.Database.EnsureCreated();

            _categoriaRepository = new CategoriaRepository(_applicationContext);
        }

        [Fact]
        [Trait("Repository", "Categoria")]
        public async Task ObterTodosAsync_DeveRetornarCategorias()
        {
            // Arrange
            _applicationContext.Categoria.AddRange(
                new CategoriaEntity { Nome = "RPG" },
                new CategoriaEntity { Nome = "Ação" });
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _categoriaRepository.ObterTodosAsync(0, 10);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Data.Count());
            Assert.Equal(2, resultado.TotalRegistros);
        }

        [Fact]
        [Trait("Repository", "Categoria")]
        public async Task ObterUmAsync_DeveRetornarUmaCategoria()
        {
            // Arrange
            var categoria = new CategoriaEntity { Nome = "RPG" };
            _applicationContext.Categoria.Add(categoria);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _categoriaRepository.ObterUmAsync(categoria.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("RPG", resultado.Nome);
        }

        [Fact]
        [Trait("Repository", "Categoria")]
        public async Task ObterUmAsync_IdInexistente_DeveRetornarNull()
        {
            var resultado = await _categoriaRepository.ObterUmAsync(99999);

            Assert.Null(resultado);
        }

        [Fact]
        [Trait("Repository", "Categoria")]
        public async Task AdicionarAsync_DeveAdicionarCategoria()
        {
            // Arrange
            var categoria = new CategoriaEntity { Nome = "RPG" };

            // Act
            var resultado = await _categoriaRepository.AdicionarAsync(categoria);

            // Assert
            Assert.NotNull(resultado);
            var categoriaNoDb = _applicationContext.Categoria.FirstOrDefault(x => x.Id == resultado!.Id);
            Assert.NotNull(categoriaNoDb);
            Assert.Equal("RPG", categoriaNoDb!.Nome);
        }

        [Fact]
        [Trait("Repository", "Categoria")]
        public async Task AdicionarAsync_NomeDuplicadoCaseInsensitive_DeveRetornarNull()
        {
            // Arrange
            _applicationContext.Categoria.Add(new CategoriaEntity { Nome = "RPG" });
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _categoriaRepository.AdicionarAsync(new CategoriaEntity { Nome = "rpg" });

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        [Trait("Repository", "Categoria")]
        public async Task DeletarAsync_DeveRemoverCategoria()
        {
            // Arrange
            var categoria = new CategoriaEntity { Nome = "RPG" };
            _applicationContext.Categoria.Add(categoria);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _categoriaRepository.DeletarAsync(categoria.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Null(_applicationContext.Categoria.FirstOrDefault(x => x.Id == categoria.Id));
        }
    }
}
