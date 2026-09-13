using Jogos.API.Application.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Jogos.API.Doc.Samples
{
    public class DesenvolvedoraRequestSample : IExamplesProvider<DesenvolvedoraRequestDto>
    {
        public DesenvolvedoraRequestDto GetExamples()
        {
            return new DesenvolvedoraRequestDto
            {
                Nome = "CD Projekt Red"
            };
        }
    }
}
