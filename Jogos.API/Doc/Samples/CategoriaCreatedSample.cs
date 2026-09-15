using Jogos.API.Application.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Jogos.API.Doc.Samples
{
    public class CategoriaCreatedSample : IExamplesProvider<CategoriaResponseDto>
    {
        public CategoriaResponseDto GetExamples()
        {
            return new CategoriaResponseDto
            {
                Id = 1,
                Nome = "RPG"
            };
        }
    }
}
