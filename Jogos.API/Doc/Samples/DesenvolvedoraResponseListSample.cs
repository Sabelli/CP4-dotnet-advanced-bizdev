using Jogos.API.Application.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Jogos.API.Doc.Samples
{
    public class DesenvolvedoraResponseListSample : IExamplesProvider<IEnumerable<DesenvolvedoraResponseDto>>
    {
        public IEnumerable<DesenvolvedoraResponseDto> GetExamples()
        {
            return new List<DesenvolvedoraResponseDto>
            {
                new DesenvolvedoraResponseDto { Id = 1, Nome = "CD Projekt Red" },
                new DesenvolvedoraResponseDto { Id = 2, Nome = "Ubisoft" }
            };
        }
    }
}
