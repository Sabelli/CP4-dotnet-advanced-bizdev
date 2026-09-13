using Jogos.API.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace Jogos.API.Doc.Samples
{
    public class PlataformaResponseListSample : IExamplesProvider<IEnumerable<PlataformaEntity>>
    {
        public IEnumerable<PlataformaEntity> GetExamples()
        {
            return new List<PlataformaEntity>
            {
                new PlataformaEntity { Id = 1, Nome = "PC" },
                new PlataformaEntity { Id = 2, Nome = "PS5" }
            };
        }
    }
}
