using Jogos.API.Application.Dtos;
using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Models;

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

        public static PageResultModel<IEnumerable<JogoResponseDto>> ToResponseDto(this PageResultModel<IEnumerable<JogoEntity>> obj)
        {
            return new PageResultModel<IEnumerable<JogoResponseDto>>
            {
                Data = obj.Data.Select(x => x.ToResponseDto()).ToList(),
                Deslocamento = obj.Deslocamento,
                RegistroRetornado = obj.RegistroRetornado,
                TotalRegistros = obj.TotalRegistros
            };
        }
    }
}
