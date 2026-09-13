using Jogos.API.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace Jogos.API.Doc.Samples
{
    public class DesenvolvedoraResponseListSample : IExamplesProvider<IEnumerable<DesenvolvedoraEntity>>
    {
        public IEnumerable<DesenvolvedoraEntity> GetExamples()
        {
            return new List<DesenvolvedoraEntity>
            {
                new DesenvolvedoraEntity
                {
                    Id = 1,
                    Nome = "CD Projekt Red",
                    Jogos = new List<JogoEntity>
                    {
                        new JogoEntity { Id = 1, Nome = "The Witcher 3", Preco = 59.99, Plataforma = "PC", DataLancamento = new DateTime(2015, 5, 19), DesenvolvedoraId = 1 },
                    }
                },
                new DesenvolvedoraEntity
                {
                    Id = 2,
                    Nome = "Ubisoft",
                    Jogos = new List<JogoEntity>
                    {
                        new JogoEntity { Id = 3, Nome = "Assassin's Creed Valhalla", Preco = 69.90, Plataforma = "PC", DataLancamento = new DateTime(2020, 11, 10), DesenvolvedoraId = 2 },
                    }
                }
            };
        }
    }
}
