using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Exceptions;
using Jogos.API.Infrastructure.Data;
using Jogos.API.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Jogos.Test.App
{
    public class JogoRepositoryTest
    {
        private readonly ApplicationContext _applicationContext;
        private readonly JogoRepository _jogoRepository;

        public JogoRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _applicationContext = new ApplicationContext(options);
            _applicationContext.Database.EnsureCreated();

            _jogoRepository = new JogoRepository(_applicationContext);
        }

        private DesenvolvedoraEntity CriarDesenvolvedora(string nome = "CD Projekt Red")
        {
            var desenvolvedora = new DesenvolvedoraEntity { Nome = nome };
            _applicationContext.Desenvolvedora.Add(desenvolvedora);
            _applicationContext.SaveChanges();
            return desenvolvedora;
        }

        [Fact]
        [Trait("Repository", "Jogo")]
        public async Task ObterTodosAsync_DeveRetornarJogos()
        {
            var desenvolvedora = CriarDesenvolvedora();
            _applicationContext.Jogo.AddRange(
                new JogoEntity { Nome = "The Witcher 3", Preco = 59.99, DataLancamento = new DateTime(2015, 5, 19), DesenvolvedoraId = desenvolvedora.Id },
                new JogoEntity { Nome = "Cyberpunk 2077", Preco = 79.90, DataLancamento = new DateTime(2020, 12, 10), DesenvolvedoraId = desenvolvedora.Id });
            _applicationContext.SaveChanges();

            var resultado = await _jogoRepository.ObterTodosAsync(0, 10);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Data.Count());
            Assert.Equal(2, resultado.TotalRegistros);
        }

        [Fact]
        [Trait("Repository", "Jogo")]
        public async Task ObterUmAsync_DeveRetornarJogoComDesenvolvedora()
        {
            var desenvolvedora = CriarDesenvolvedora();
            var jogo = new JogoEntity { Nome = "The Witcher 3", Preco = 59.99, DataLancamento = new DateTime(2015, 5, 19), DesenvolvedoraId = desenvolvedora.Id };
            _applicationContext.Jogo.Add(jogo);
            _applicationContext.SaveChanges();

            var resultado = await _jogoRepository.ObterUmAsync(jogo.Id);

            Assert.NotNull(resultado);
            Assert.Equal("The Witcher 3", resultado.Nome);
            Assert.NotNull(resultado.Desenvolvedora);
            Assert.Equal(desenvolvedora.Id, resultado.Desenvolvedora!.Id);
        }

        [Fact]
        [Trait("Repository", "Jogo")]
        public async Task AdicionarAsync_ComCategoriaIds_DeveVincularCategoriasExistentes()
        {
            var desenvolvedora = CriarDesenvolvedora();
            var categoria = new CategoriaEntity { Nome = "RPG" };
            _applicationContext.Categoria.Add(categoria);
            _applicationContext.SaveChanges();

            var jogo = new JogoEntity { Nome = "The Witcher 3", Preco = 59.99, DataLancamento = new DateTime(2015, 5, 19), DesenvolvedoraId = desenvolvedora.Id };

            var resultado = await _jogoRepository.AdicionarAsync(jogo, new List<int> { categoria.Id }, null);

            Assert.NotNull(resultado);
            Assert.NotNull(resultado!.Categorias);
            Assert.Single(resultado.Categorias!);
            Assert.Equal(categoria.Id, resultado.Categorias!.First().Id);

            // Confirma que não duplicou a categoria (deveria continuar existindo só 1 no banco)
            Assert.Single(_applicationContext.Categoria);
        }

        [Fact]
        [Trait("Repository", "Jogo")]
        public async Task AdicionarAsync_NomeDuplicadoCaseInsensitive_DeveRetornarNull()
        {
            var desenvolvedora = CriarDesenvolvedora();
            _applicationContext.Jogo.Add(new JogoEntity { Nome = "The Witcher 3", Preco = 59.99, DataLancamento = new DateTime(2015, 5, 19), DesenvolvedoraId = desenvolvedora.Id });
            _applicationContext.SaveChanges();

            var resultado = await _jogoRepository.AdicionarAsync(
                new JogoEntity { Nome = "the witcher 3", Preco = 10, DataLancamento = DateTime.Now, DesenvolvedoraId = desenvolvedora.Id },
                null, null);

            Assert.Null(resultado);
        }

        [Fact]
        [Trait("Repository", "Jogo")]
        public async Task VincularCategoriaAsync_DeveAdicionarCategoriaAoJogo()
        {
            // Arrange
            var desenvolvedora = CriarDesenvolvedora();
            var categoria = new CategoriaEntity { Nome = "RPG" };
            var jogo = new JogoEntity { Nome = "The Witcher 3", Preco = 59.99, DataLancamento = new DateTime(2015, 5, 19), DesenvolvedoraId = desenvolvedora.Id };
            _applicationContext.Categoria.Add(categoria);
            _applicationContext.Jogo.Add(jogo);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _jogoRepository.VincularCategoriaAsync(jogo.Id, [categoria.Id]);

            // Assert
            Assert.NotNull(resultado);
            Assert.Contains(resultado!.Categorias!, x => x.Id == categoria.Id);
        }

        [Fact]
        [Trait("Repository", "Jogo")]
        public async Task VincularCategoriaAsync_CategoriaInexistente_DeveLancarExcecao()
        {
            // Arrange
            var desenvolvedora = CriarDesenvolvedora();
            var jogo = new JogoEntity { Nome = "The Witcher 3", Preco = 59.99, DataLancamento = new DateTime(2015, 5, 19), DesenvolvedoraId = desenvolvedora.Id };
            _applicationContext.Jogo.Add(jogo);
            _applicationContext.SaveChanges();

            // Act
            var excecao = await Record.ExceptionAsync(() => _jogoRepository.VincularCategoriaAsync(jogo.Id, [99999]));

            // Assert
            Assert.IsType<EntidadeNaoEncontradaException>(excecao);
        }

        [Fact]
        [Trait("Repository", "Jogo")]
        public async Task DesvincularCategoriaAsync_DeveRemoverCategoriaDoJogo()
        {
            // Arrange
            var desenvolvedora = CriarDesenvolvedora();
            var categoria = new CategoriaEntity { Nome = "RPG" };
            _applicationContext.Categoria.Add(categoria);
            _applicationContext.SaveChanges();

            var jogo = new JogoEntity { Nome = "The Witcher 3", Preco = 59.99, DataLancamento = new DateTime(2015, 5, 19), DesenvolvedoraId = desenvolvedora.Id };
            await _jogoRepository.AdicionarAsync(jogo, new List<int> { categoria.Id }, null);

            // Act
            var resultado = await _jogoRepository.DesvincularCategoriaAsync(jogo.Id, [categoria.Id]);

            // Assert
            Assert.NotNull(resultado);
            Assert.DoesNotContain(resultado!.Categorias ?? [], x => x.Id == categoria.Id);
        }
    }
}
