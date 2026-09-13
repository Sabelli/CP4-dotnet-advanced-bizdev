using Jogos.API.Application.Dtos;
using Jogos.API.Domain.Entities;

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
    }
}
