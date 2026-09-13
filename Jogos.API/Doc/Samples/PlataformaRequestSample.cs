using Jogos.API.Application.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Jogos.API.Doc.Samples
{
    public class PlataformaRequestSample : IExamplesProvider<PlataformaRequestDto>
    {
        public PlataformaRequestDto GetExamples()
        {
            return new PlataformaRequestDto
            {
                Nome = "PC"
            };
        }
    }
}
