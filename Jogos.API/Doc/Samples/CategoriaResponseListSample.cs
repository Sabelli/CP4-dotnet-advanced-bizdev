using Jogos.API.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace Jogos.API.Doc.Samples
{
    public class CategoriaResponseListSample : IExamplesProvider<IEnumerable<CategoriaEntity>>
    {
        public IEnumerable<CategoriaEntity> GetExamples()
        {
            return new List<CategoriaEntity>
            {
                new CategoriaEntity { Id = 1, Nome = "RPG" },
                new CategoriaEntity { Id = 2, Nome = "Ação" }
            };
        }
    }
}
