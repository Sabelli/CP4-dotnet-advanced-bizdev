using Jogos.API.Application.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Jogos.API.Doc.Samples
{
    public class PlataformaResponseListSample : IExamplesProvider<IEnumerable<PlataformaResponseDto>>
    {
        public IEnumerable<PlataformaResponseDto> GetExamples()
        {
            return new List<PlataformaResponseDto>
            {
                new PlataformaResponseDto { Id = 1, Nome = "PC" },
                new PlataformaResponseDto { Id = 2, Nome = "PS5" }
            };
        }
    }
}
