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
                DataLancamento = obj.DataLancamento,
                DesenvolvedoraId = obj.DesenvolvedoraId
            };
        }

        public static JogoEntity ToJogoEntity(this JogoUpdateRequestDto obj)
        {
            return new JogoEntity
            {
                Nome = obj.Nome,
                Preco = obj.Preco,
                DataLancamento = obj.DataLancamento,
                DesenvolvedoraId = obj.DesenvolvedoraId
            };
        }

        public static JogoResponseDto ToResponseDto(this JogoEntity obj)
        {
            return new JogoResponseDto
            {
                Id = obj.Id,
                Nome = obj.Nome,
                Preco = obj.Preco,
                DataLancamento = obj.DataLancamento,
                DesenvolvedoraId = obj.DesenvolvedoraId,
                Desenvolvedora = obj.Desenvolvedora,
                Categorias = obj.Categorias,
                Plataformas = obj.Plataformas
            };
        }
    }
}
