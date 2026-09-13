using Jogos.API.Application.Dtos;
using Jogos.API.Domain.Entities;

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
    }
}
