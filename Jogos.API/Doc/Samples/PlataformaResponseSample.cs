using Jogos.API.Application.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Jogos.API.Doc.Samples
{
    public class PlataformaResponseSample : IExamplesProvider<PlataformaResponseDto>
    {
        public PlataformaResponseDto GetExamples()
        {
            return new PlataformaResponseDto
            {
                Id = 1,
                Nome = "PC"
            };
        }
    }
}
