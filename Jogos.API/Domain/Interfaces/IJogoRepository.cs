using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Models;

namespace Jogos.API.Domain.Interfaces
{
    public interface IJogoRepository
    {
        Task<PageResultModel<IEnumerable<JogoEntity>>> ObterTodosAsync(int Deslocamento, int RegistroRetornado);
        JogoEntity? ObterUm(int Id);
        JogoEntity? Adicionar(JogoEntity entity);
        JogoEntity? Editar(int Id, JogoEntity entity);
        JogoEntity? Deletar(int Id);
    }
}
