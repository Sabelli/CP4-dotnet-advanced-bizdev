using Jogos.API.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace Jogos.API.Doc.Samples
{
    public class DesenvolvedoraCreatedSample : IExamplesProvider<DesenvolvedoraEntity>
    {
        public DesenvolvedoraEntity GetExamples()
        {
            return new DesenvolvedoraEntity
            {
                Id = 1,
                Nome = "CD Projekt Red"
            };
        }
    }
}
