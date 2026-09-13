using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Models;

namespace Jogos.API.Domain.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<PageResultModel<IEnumerable<CategoriaEntity>>> ObterTodosAsync(int Deslocamento, int RegistroRetornado);
        Task<CategoriaEntity?> ObterUmAsync(int Id);
        Task<CategoriaEntity?> AdicionarAsync(CategoriaEntity entity);
        Task<CategoriaEntity?> EditarAsync(int Id, CategoriaEntity entity);
        Task<CategoriaEntity?> DeletarAsync(int Id);
    }
}
