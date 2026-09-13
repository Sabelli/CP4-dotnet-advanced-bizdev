using Jogos.API.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace Jogos.API.Doc.Samples
{
    public class PlataformaResponseSample : IExamplesProvider<PlataformaEntity>
    {
        public PlataformaEntity GetExamples()
        {
            return new PlataformaEntity
            {
                Id = 1,
                Nome = "PC"
            };
        }
    }
}
