using Jogos.API.Application.Dtos;
using Jogos.API.Domain.Entities;

namespace Jogos.API.Application.Mappers
{
    public static class JogoMapper
    {
        public static JogoEntity ToJogoEntity(this JogoRequestDto obj)
        {
            return new JogoEntity
            {
                Nome = obj.Nome,
                Preco = obj.Preco,
                Plataforma = obj.Plataforma,
                DataLancamento = obj.DataLancamento,
                DesenvolvedoraId = obj.DesenvolvedoraId
            };
        }
    }
}
