using Jogos.API.Domain.Entities;
using Jogos.API.Infrastructure.Data;
using Jogos.API.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Jogos.Test.App
{
    public class DesenvolvedoraRepositoryTest
    {
        private readonly ApplicationContext _applicationContext;
        private readonly DesenvolvedoraRepository _desenvolvedoraRepository;

        public DesenvolvedoraRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _applicationContext = new ApplicationContext(options);
            _applicationContext.Database.EnsureCreated();

            _desenvolvedoraRepository = new DesenvolvedoraRepository(_applicationContext);
        }

        [Fact]
        [Trait("Repository", "Desenvolvedora")]
        public async Task ObterTodosAsync_DeveRetornarDesenvolvedoras()
        {
            _applicationContext.Desenvolvedora.AddRange(
                new DesenvolvedoraEntity { Nome = "CD Projekt Red" },
                new DesenvolvedoraEntity { Nome = "Ubisoft" });
            _applicationContext.SaveChanges();

            var resultado = await _desenvolvedoraRepository.ObterTodosAsync(0, 10);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Data.Count());
            Assert.Equal(2, resultado.TotalRegistros);
        }

        [Fact]
        [Trait("Repository", "Desenvolvedora")]
        public async Task ObterUmAsync_DeveRetornarUmaDesenvolvedora()
        {
            var desenvolvedora = new DesenvolvedoraEntity { Nome = "CD Projekt Red" };
            _applicationContext.Desenvolvedora.Add(desenvolvedora);
            _applicationContext.SaveChanges();

            var resultado = await _desenvolvedoraRepository.ObterUmAsync(desenvolvedora.Id);

            Assert.NotNull(resultado);
            Assert.Equal("CD Projekt Red", resultado.Nome);
        }

        [Fact]
        [Trait("Repository", "Desenvolvedora")]
        public async Task AdicionarAsync_DeveAdicionarDesenvolvedora()
        {
            var desenvolvedora = new DesenvolvedoraEntity { Nome = "CD Projekt Red" };

            var resultado = await _desenvolvedoraRepository.AdicionarAsync(desenvolvedora);

            Assert.NotNull(resultado);
            var noDb = _applicationContext.Desenvolvedora.FirstOrDefault(x => x.Id == resultado!.Id);
            Assert.NotNull(noDb);
        }

        [Fact]
        [Trait("Repository", "Desenvolvedora")]
        public async Task AdicionarAsync_NomeDuplicadoCaseInsensitive_DeveRetornarNull()
        {
            _applicationContext.Desenvolvedora.Add(new DesenvolvedoraEntity { Nome = "CD Projekt Red" });
            _applicationContext.SaveChanges();

            var resultado = await _desenvolvedoraRepository.AdicionarAsync(new DesenvolvedoraEntity { Nome = "cd projekt red" });

            Assert.Null(resultado);
        }

        [Fact]
        [Trait("Repository", "Desenvolvedora")]
        public async Task DeletarAsync_DeveRemoverDesenvolvedora()
        {
            var desenvolvedora = new DesenvolvedoraEntity { Nome = "CD Projekt Red" };
            _applicationContext.Desenvolvedora.Add(desenvolvedora);
            _applicationContext.SaveChanges();

            var resultado = await _desenvolvedoraRepository.DeletarAsync(desenvolvedora.Id);

            Assert.NotNull(resultado);
            Assert.Null(_applicationContext.Desenvolvedora.FirstOrDefault(x => x.Id == desenvolvedora.Id));
        }
    }
}
