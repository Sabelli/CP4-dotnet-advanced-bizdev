using Jogos.API.Application.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Jogos.API.Doc.Samples
{
    public class CategoriaResponseListSample : IExamplesProvider<IEnumerable<CategoriaResponseDto>>
    {
        public IEnumerable<CategoriaResponseDto> GetExamples()
        {
            return new List<CategoriaResponseDto>
            {
                new CategoriaResponseDto { Id = 1, Nome = "RPG" },
                new CategoriaResponseDto { Id = 2, Nome = "Ação" }
            };
        }
    }
}
