using Jogos.API.Application.Dtos;
using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Models;

namespace Jogos.API.Application.Mappers
{
    public static class CategoriaMapper
    {
        public static CategoriaEntity ToCategoriaEntity(this CategoriaRequestDto obj)
        {
            return new CategoriaEntity
            {
                Nome = obj.Nome
            };
        }

        public static CategoriaResponseDto ToResponseDto(this CategoriaEntity obj)
        {
            return new CategoriaResponseDto
            {
                Id = obj.Id,
                Nome = obj.Nome
            };
        }

        public static PageResultModel<IEnumerable<CategoriaResponseDto>> ToResponseDto(this PageResultModel<IEnumerable<CategoriaEntity>> obj)
        {
            return new PageResultModel<IEnumerable<CategoriaResponseDto>>
            {
                Data = obj.Data.Select(x => x.ToResponseDto()).ToList(),
                Deslocamento = obj.Deslocamento,
                RegistroRetornado = obj.RegistroRetornado,
                TotalRegistros = obj.TotalRegistros
            };
        }
    }
}
