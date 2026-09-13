using Jogos.API.Application.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Jogos.API.Doc.Samples
{
    public class CategoriaRequestSample : IExamplesProvider<CategoriaRequestDto>
    {
        public CategoriaRequestDto GetExamples()
        {
            return new CategoriaRequestDto
            {
                Nome = "RPG"
            };
        }
    }
}
