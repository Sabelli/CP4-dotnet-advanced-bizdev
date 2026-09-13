using Jogos.API.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace Jogos.API.Doc.Samples
{
    public class DesenvolvedoraResponseSample : IExamplesProvider<DesenvolvedoraEntity>
    {
        public DesenvolvedoraEntity GetExamples()
        {
            return new DesenvolvedoraEntity
            {
                Id = 1,
                Nome = "CD Projekt Red",
                Jogos = new List<JogoEntity>
                {
                    new JogoEntity { Id = 1, Nome = "The Witcher 3", Preco = 59.99, Plataforma = "PC", DataLancamento = new DateTime(2015, 5, 19), DesenvolvedoraId = 1 },
                    new JogoEntity { Id = 2, Nome = "Cyberpunk 2077", Preco = 79.90, Plataforma = "PC", DataLancamento = new DateTime(2020, 12, 10), DesenvolvedoraId = 1 },
                }
            };
        }
    }
}
