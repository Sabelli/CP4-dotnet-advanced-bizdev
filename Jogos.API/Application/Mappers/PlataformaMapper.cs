using Jogos.API.Application.Dtos;
using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Models;

namespace Jogos.API.Application.Mappers
{
    public static class PlataformaMapper
    {
        public static PlataformaEntity ToPlataformaEntity(this PlataformaRequestDto obj)
        {
            return new PlataformaEntity
            {
                Nome = obj.Nome
            };
        }

        public static PlataformaResponseDto ToResponseDto(this PlataformaEntity obj)
        {
            return new PlataformaResponseDto
            {
                Id = obj.Id,
                Nome = obj.Nome
            };
        }

        public static PageResultModel<IEnumerable<PlataformaResponseDto>> ToResponseDto(this PageResultModel<IEnumerable<PlataformaEntity>> obj)
        {
            return new PageResultModel<IEnumerable<PlataformaResponseDto>>
            {
                Data = obj.Data.Select(x => x.ToResponseDto()).ToList(),
                Deslocamento = obj.Deslocamento,
                RegistroRetornado = obj.RegistroRetornado,
                TotalRegistros = obj.TotalRegistros
            };
        }
    }
}
