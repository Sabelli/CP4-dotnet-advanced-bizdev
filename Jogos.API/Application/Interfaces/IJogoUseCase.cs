using Jogos.API.Application.Dtos;
using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Models;

namespace Jogos.API.Application.Interfaces
{
    public interface IJogoUseCase
    {
        Task<PageResultModel<IEnumerable<JogoEntity>>> ObterTodosJogosAsync(int Deslocamento, int RegistroRetornado);
        Task<JogoEntity?> ObterUmJogoAsync(int Id);
        Task<IEnumerable<JogoEntity>> ObterJogosPorNomeAsync(string nome);
        Task<IEnumerable<JogoEntity>> ObterJogosPorPlataformaAsync(string plataforma);
        Task<IEnumerable<JogoEntity>> ObterJogosPorDesenvolvedoraAsync(int idDesenvolvedora);
        Task<IEnumerable<JogoEntity>> ObterJogosPorCategoriaAsync(int idCategoria);
        Task<JogoEntity?> AdicionarJogoAsync(JogoRequestDto entity);
        Task<JogoEntity?> EditarJogoAsync(int Id, JogoRequestDto entity);
        Task<JogoEntity?> DeletarJogoAsync(int Id);
        Task<JogoEntity?> VincularCategoriaAsync(int idJogo, int idCategoria);
    }
}
