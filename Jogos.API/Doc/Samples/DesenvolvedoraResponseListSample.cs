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
                new DesenvolvedoraEntity { Id = 1, Nome = "CD Projekt Red" },
                new DesenvolvedoraEntity { Id = 2, Nome = "Ubisoft" }
            };
        }
    }
}
