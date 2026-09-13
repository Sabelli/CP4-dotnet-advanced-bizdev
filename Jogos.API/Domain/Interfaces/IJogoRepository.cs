using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Models;

namespace Jogos.API.Domain.Interfaces
{
    public interface IJogoRepository
    {
        Task<PageResultModel<IEnumerable<JogoEntity>>> ObterTodosAsync(int Deslocamento, int RegistroRetornado);
        Task<JogoEntity?> ObterUmAsync(int Id);
        Task<IEnumerable<JogoEntity>> ObterPorNomeAsync(string nome);
        Task<IEnumerable<JogoEntity>> ObterPorPlataformaAsync(string plataforma);
        Task<IEnumerable<JogoEntity>> ObterPorDesenvolvedoraAsync(int idDesenvolvedora);
        Task<IEnumerable<JogoEntity>> ObterPorCategoriaAsync(int idCategoria);
        Task<JogoEntity?> AdicionarAsync(JogoEntity entity);
        Task<JogoEntity?> EditarAsync(int Id, JogoEntity entity);
        Task<JogoEntity?> DeletarAsync(int Id);
        Task<JogoEntity?> VincularCategoriaAsync(int idJogo, int idCategoria);
    }
}
