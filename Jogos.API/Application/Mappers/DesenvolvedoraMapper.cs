using Jogos.API.Application.Dtos;
using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Models;

namespace Jogos.API.Application.Mappers
{
    public static class DesenvolvedoraMapper
    {
        public static DesenvolvedoraEntity ToDesenvolvedoraEntity(this DesenvolvedoraRequestDto obj)
        {
            return new DesenvolvedoraEntity
            {
                Nome = obj.Nome
            };
        }

        public static DesenvolvedoraResponseDto ToResponseDto(this DesenvolvedoraEntity obj)
        {
            return new DesenvolvedoraResponseDto
            {
                Id = obj.Id,
                Nome = obj.Nome
            };
        }

        public static PageResultModel<IEnumerable<DesenvolvedoraResponseDto>> ToResponseDto(this PageResultModel<IEnumerable<DesenvolvedoraEntity>> obj)
        {
            return new PageResultModel<IEnumerable<DesenvolvedoraResponseDto>>
            {
                Data = obj.Data.Select(x => x.ToResponseDto()).ToList(),
                Deslocamento = obj.Deslocamento,
                RegistroRetornado = obj.RegistroRetornado,
                TotalRegistros = obj.TotalRegistros
            };
        }
    }
}
