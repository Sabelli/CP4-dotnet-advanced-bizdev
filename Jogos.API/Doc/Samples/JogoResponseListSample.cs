using Jogos.API.Application.Dtos;
using Jogos.API.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace Jogos.API.Doc.Samples
{
    public class JogoResponseListSample : IExamplesProvider<IEnumerable<JogoResponseDto>>
    {
        public IEnumerable<JogoResponseDto> GetExamples()
        {
            return new List<JogoResponseDto>
            {
                new JogoResponseDto
                {
                    Id = 1,
                    Nome = "The Witcher 3",
                    Preco = 59.99,
                    DataLancamento = new DateTime(2015, 5, 19),
                    DesenvolvedoraId = 1,
                    Desenvolvedora = new DesenvolvedoraEntity { Id = 1, Nome = "CD Projekt Red" },
                    Categorias = new List<CategoriaEntity>
                    {
                        new CategoriaEntity { Id = 1, Nome = "RPG" },
                    },
                    Plataformas = new List<PlataformaEntity>
                    {
                        new PlataformaEntity { Id = 1, Nome = "PC" },
                        new PlataformaEntity { Id = 2, Nome = "PS5" },
                    }
                },
                new JogoResponseDto
                {
                    Id = 2,
                    Nome = "Cyberpunk 2077",
                    Preco = 79.90,
                    DataLancamento = new DateTime(2020, 12, 10),
                    DesenvolvedoraId = 1,
                    Desenvolvedora = new DesenvolvedoraEntity { Id = 1, Nome = "CD Projekt Red" },
                    Categorias = new List<CategoriaEntity>
                    {
                        new CategoriaEntity { Id = 2, Nome = "Ação" },
                    },
                    Plataformas = new List<PlataformaEntity>
                    {
                        new PlataformaEntity { Id = 1, Nome = "PC" },
                    }
                }
            };
        }
    }
}
