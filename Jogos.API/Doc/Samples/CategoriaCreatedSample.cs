using Jogos.API.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace Jogos.API.Doc.Samples
{
    public class CategoriaCreatedSample : IExamplesProvider<CategoriaEntity>
    {
        public CategoriaEntity GetExamples()
        {
            return new CategoriaEntity
            {
                Id = 1,
                Nome = "RPG"
            };
        }
    }
}
