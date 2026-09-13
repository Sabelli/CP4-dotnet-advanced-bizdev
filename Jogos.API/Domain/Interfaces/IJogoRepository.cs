using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Models;

namespace Jogos.API.Domain.Interfaces
{
    public interface IJogoRepository
    {
        Task<PageResultModel<IEnumerable<JogoEntity>>> ObterTodosAsync(int Deslocamento, int RegistroRetornado);
        Task<JogoEntity?> ObterUmAsync(int Id);
        Task<IEnumerable<JogoEntity>> ObterPorNomeAsync(string nome, int Deslocamento, int RegistroRetornado);
        Task<IEnumerable<JogoEntity>> ObterPorPlataformaAsync(string plataforma, int Deslocamento, int RegistroRetornado);
        Task<IEnumerable<JogoEntity>> ObterPorDesenvolvedoraAsync(int idDesenvolvedora, int Deslocamento, int RegistroRetornado);
        Task<IEnumerable<JogoEntity>> ObterPorCategoriaAsync(int idCategoria, int Deslocamento, int RegistroRetornado);
        Task<JogoEntity?> AdicionarAsync(JogoEntity entity, IEnumerable<int>? categoriaIds, IEnumerable<int>? plataformaIds);
        Task<JogoEntity?> EditarAsync(int Id, JogoEntity entity);
        Task<JogoEntity?> DeletarAsync(int Id);
        Task<JogoEntity?> VincularCategoriaAsync(int idJogo, int idCategoria);
        Task<JogoEntity?> DesvincularCategoriaAsync(int idJogo, int idCategoria);
        Task<JogoEntity?> VincularPlataformaAsync(int idJogo, int idPlataforma);
        Task<JogoEntity?> DesvincularPlataformaAsync(int idJogo, int idPlataforma);
    }
}
