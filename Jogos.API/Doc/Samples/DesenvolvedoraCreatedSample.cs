using Jogos.API.Application.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Jogos.API.Doc.Samples
{
    public class DesenvolvedoraCreatedSample : IExamplesProvider<DesenvolvedoraResponseDto>
    {
        public DesenvolvedoraResponseDto GetExamples()
        {
            return new DesenvolvedoraResponseDto
            {
                Id = 1,
                Nome = "CD Projekt Red"
            };
        }
    }
}
