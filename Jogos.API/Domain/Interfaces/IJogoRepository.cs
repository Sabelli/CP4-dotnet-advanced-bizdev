using Jogos.API.Domain.Entities;

namespace Jogos.API.Domain.Interfaces
{
    public interface IJogoRepository
    {
        Task<IEnumerable<JogoEntity>> ObterTodosAsync(int Deslocamento, int RegistroRetornado);
        JogoEntity? ObterUm(int Id);
        JogoEntity? Adicionar(JogoEntity entity);
        JogoEntity? Editar(int Id, JogoEntity entity);
        JogoEntity? Deletar(int Id);
    }
}
