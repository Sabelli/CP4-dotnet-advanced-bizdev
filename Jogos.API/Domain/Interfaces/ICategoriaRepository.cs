using Jogos.API.Domain.Entities;

namespace Jogos.API.Domain.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<IEnumerable<CategoriaEntity>> ObterTodosAsync(int Deslocamento, int RegistroRetornado);
        CategoriaEntity? ObterUm(int Id);
        CategoriaEntity? Adicionar(CategoriaEntity entity);
        CategoriaEntity? Editar(int Id, CategoriaEntity entity);
        CategoriaEntity? Deletar(int Id);
    }
}
