using Jogos.API.Application.Dtos;
using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Models;

namespace Jogos.API.Application.Interfaces
{
    public interface IJogoUseCase
    {
        Task<PageResultModel<IEnumerable<JogoEntity>>> ObterTodosJogosAsync(int Deslocamento, int RegistroRetornado);
        Task<JogoEntity?> ObterUmJogoAsync(int Id);
        Task<IEnumerable<JogoEntity>> ObterJogosPorNomeAsync(string nome, int Deslocamento, int RegistroRetornado);
        Task<IEnumerable<JogoEntity>> ObterJogosPorPlataformaAsync(string plataforma, int Deslocamento, int RegistroRetornado);
        Task<IEnumerable<JogoEntity>> ObterJogosPorDesenvolvedoraAsync(int idDesenvolvedora, int Deslocamento, int RegistroRetornado);
        Task<IEnumerable<JogoEntity>> ObterJogosPorCategoriaAsync(int idCategoria, int Deslocamento, int RegistroRetornado);
        Task<JogoEntity?> AdicionarJogoAsync(JogoRequestDto entity);
        Task<JogoEntity?> EditarJogoAsync(int Id, JogoUpdateRequestDto entity);
        Task<JogoEntity?> DeletarJogoAsync(int Id);
        Task<JogoEntity?> VincularCategoriaAsync(int idJogo, IEnumerable<int> categoriaIds);
        Task<JogoEntity?> DesvincularCategoriaAsync(int idJogo, IEnumerable<int> categoriaIds);
        Task<JogoEntity?> VincularPlataformaAsync(int idJogo, IEnumerable<int> plataformaIds);
        Task<JogoEntity?> DesvincularPlataformaAsync(int idJogo, IEnumerable<int> plataformaIds);
    }
}
