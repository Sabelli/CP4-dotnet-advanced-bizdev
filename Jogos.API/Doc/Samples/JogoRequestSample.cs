using Jogos.API.Application.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Jogos.API.Doc.Samples
{
    public class JogoRequestSample : IExamplesProvider<JogoRequestDto>
    {
        public JogoRequestDto GetExamples()
        {
            return new JogoRequestDto
            {
                Nome = "The Witcher 3",
                Preco = 59.99,
                DataLancamento = new DateTime(2015, 5, 19),
                DesenvolvedoraId = 1,
                CategoriaIds = new List<int> { 1, 2 },
                PlataformaIds = new List<int> { 1, 2 }
            };
        }
    }
}
