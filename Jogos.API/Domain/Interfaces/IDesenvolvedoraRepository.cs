using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Models;

namespace Jogos.API.Domain.Interfaces
{
    public interface IDesenvolvedoraRepository
    {
        Task<PageResultModel<IEnumerable<DesenvolvedoraEntity>>> ObterTodosAsync(int Deslocamento, int RegistroRetornado);
        DesenvolvedoraEntity? ObterUm(int Id);
        DesenvolvedoraEntity? Adicionar(DesenvolvedoraEntity entity);
        DesenvolvedoraEntity? Editar(int Id, DesenvolvedoraEntity entity);
        DesenvolvedoraEntity? Deletar(int Id);
    }
}
