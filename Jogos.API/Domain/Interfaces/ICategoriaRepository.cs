using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Models;

namespace Jogos.API.Domain.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<PageResultModel<IEnumerable<CategoriaEntity>>> ObterTodosAsync(int Deslocamento, int RegistroRetornado);
        CategoriaEntity? ObterUm(int Id);
        CategoriaEntity? Adicionar(CategoriaEntity entity);
        CategoriaEntity? Editar(int Id, CategoriaEntity entity);
        CategoriaEntity? Deletar(int Id);
    }
}
