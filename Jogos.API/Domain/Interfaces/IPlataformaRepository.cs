using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Models;

namespace Jogos.API.Domain.Interfaces
{
    public interface IPlataformaRepository
    {
        Task<PageResultModel<IEnumerable<PlataformaEntity>>> ObterTodosAsync(int Deslocamento, int RegistroRetornado);
        Task<PlataformaEntity?> ObterUmAsync(int Id);
        Task<PlataformaEntity?> AdicionarAsync(PlataformaEntity entity);
        Task<PlataformaEntity?> EditarAsync(int Id, PlataformaEntity entity);
        Task<PlataformaEntity?> DeletarAsync(int Id);
    }
}
