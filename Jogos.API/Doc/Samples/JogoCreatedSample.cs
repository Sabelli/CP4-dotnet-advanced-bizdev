using Jogos.API.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace Jogos.API.Doc.Samples
{
    public class JogoCreatedSample : IExamplesProvider<JogoEntity>
    {
        public JogoEntity GetExamples()
        {
            return new JogoEntity
            {
                Id = 1,
                Nome = "The Witcher 3",
                Preco = 59.99,
                DataLancamento = new DateTime(2015, 5, 19),
                DesenvolvedoraId = 1
            };
        }
    }
}
