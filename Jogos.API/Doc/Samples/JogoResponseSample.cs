using Jogos.API.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace Jogos.API.Doc.Samples
{
    public class JogoResponseSample : IExamplesProvider<JogoEntity>
    {
        public JogoEntity GetExamples()
        {
            return new JogoEntity
            {
                Id = 1,
                Nome = "The Witcher 3",
                Preco = 59.99,
                Plataforma = "PC",
                DataLancamento = new DateTime(2015, 5, 19),
                DesenvolvedoraId = 1,
                Desenvolvedora = new DesenvolvedoraEntity { Id = 1, Nome = "CD Projekt Red" },
                Categorias = new List<CategoriaEntity>
                {
                    new CategoriaEntity { Id = 1, Nome = "RPG" },
                    new CategoriaEntity { Id = 2, Nome = "Ação" },
                }
            };
        }
    }
}
