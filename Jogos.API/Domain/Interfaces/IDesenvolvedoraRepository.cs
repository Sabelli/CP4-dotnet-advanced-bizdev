using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Models;

namespace Jogos.API.Domain.Interfaces
{
    public interface IDesenvolvedoraRepository
    {
        Task<PageResultModel<IEnumerable<DesenvolvedoraEntity>>> ObterTodosAsync(int Deslocamento, int RegistroRetornado);
        Task<DesenvolvedoraEntity?> ObterUmAsync(int Id);
        Task<DesenvolvedoraEntity?> AdicionarAsync(DesenvolvedoraEntity entity);
        Task<DesenvolvedoraEntity?> EditarAsync(int Id, DesenvolvedoraEntity entity);
        Task<DesenvolvedoraEntity?> DeletarAsync(int Id);
    }
}
