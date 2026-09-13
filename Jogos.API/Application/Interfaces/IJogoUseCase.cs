using Jogos.API.Application.Dtos;
using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Models;

namespace Jogos.API.Application.Interfaces
{
    public interface IJogoUseCase
    {
        Task<PageResultModel<IEnumerable<JogoEntity>>> ObterTodosJogosAsync(int Deslocamento, int RegistroRetornado);
        JogoEntity? ObterUmJogo(int Id);
        JogoEntity? AdicionarJogo(JogoRequestDto entity);
        JogoEntity? EditarJogo(int Id, JogoRequestDto entity);
        JogoEntity? DeletarJogo(int Id);
    }
}
