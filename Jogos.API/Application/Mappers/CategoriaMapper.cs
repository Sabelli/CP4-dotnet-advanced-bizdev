using Jogos.API.Application.Dtos;
using Jogos.API.Domain.Entities;

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
    }
}
