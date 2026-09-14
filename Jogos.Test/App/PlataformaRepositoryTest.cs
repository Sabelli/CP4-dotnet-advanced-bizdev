using Jogos.API.Domain.Entities;
using Jogos.API.Infrastructure.Data;
using Jogos.API.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Jogos.Test.App
{
    public class PlataformaRepositoryTest
    {
        private readonly ApplicationContext _applicationContext;
        private readonly PlataformaRepository _plataformaRepository;

        public PlataformaRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _applicationContext = new ApplicationContext(options);
            _applicationContext.Database.EnsureCreated();

            _plataformaRepository = new PlataformaRepository(_applicationContext);
        }

        [Fact]
        [Trait("Repository", "Plataforma")]
        public async Task ObterTodosAsync_DeveRetornarPlataformas()
        {
            _applicationContext.Plataforma.AddRange(
                new PlataformaEntity { Nome = "PC" },
                new PlataformaEntity { Nome = "PS5" });
            _applicationContext.SaveChanges();

            var resultado = await _plataformaRepository.ObterTodosAsync(0, 10);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Data.Count());
            Assert.Equal(2, resultado.TotalRegistros);
        }

        [Fact]
        [Trait("Repository", "Plataforma")]
        public async Task ObterUmAsync_DeveRetornarUmaPlataforma()
        {
            var plataforma = new PlataformaEntity { Nome = "PC" };
            _applicationContext.Plataforma.Add(plataforma);
            _applicationContext.SaveChanges();

            var resultado = await _plataformaRepository.ObterUmAsync(plataforma.Id);

            Assert.NotNull(resultado);
            Assert.Equal("PC", resultado.Nome);
        }

        [Fact]
        [Trait("Repository", "Plataforma")]
        public async Task AdicionarAsync_NomeCurtoDoisCaracteres_DeveAdicionar()
        {
            var plataforma = new PlataformaEntity { Nome = "PC" };

            var resultado = await _plataformaRepository.AdicionarAsync(plataforma);

            Assert.NotNull(resultado);
            Assert.Equal("PC", resultado!.Nome);
        }

        [Fact]
        [Trait("Repository", "Plataforma")]
        public async Task AdicionarAsync_NomeDuplicadoCaseInsensitive_DeveRetornarNull()
        {
            _applicationContext.Plataforma.Add(new PlataformaEntity { Nome = "PC" });
            _applicationContext.SaveChanges();

            var resultado = await _plataformaRepository.AdicionarAsync(new PlataformaEntity { Nome = "pc" });

            Assert.Null(resultado);
        }

        [Fact]
        [Trait("Repository", "Plataforma")]
        public async Task DeletarAsync_DeveRemoverPlataforma()
        {
            var plataforma = new PlataformaEntity { Nome = "PC" };
            _applicationContext.Plataforma.Add(plataforma);
            _applicationContext.SaveChanges();

            var resultado = await _plataformaRepository.DeletarAsync(plataforma.Id);

            Assert.NotNull(resultado);
            Assert.Null(_applicationContext.Plataforma.FirstOrDefault(x => x.Id == plataforma.Id));
        }
    }
}
