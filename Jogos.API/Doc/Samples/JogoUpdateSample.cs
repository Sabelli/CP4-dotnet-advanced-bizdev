using Jogos.API.Application.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Jogos.API.Doc.Samples
{
    public class JogoUpdateSample : IExamplesProvider<JogoUpdateRequestDto>
    {
        public JogoUpdateRequestDto GetExamples()
        {
            return new JogoUpdateRequestDto
            {
                Nome = "The Witcher 3",
                Preco = 59.99,
                DataLancamento = new DateTime(2015, 5, 19),
                DesenvolvedoraId = 1
            };
        }
    }
}
