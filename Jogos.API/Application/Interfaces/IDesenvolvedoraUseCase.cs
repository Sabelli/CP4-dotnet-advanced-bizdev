using Jogos.API.Application.Dtos;
using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Models;

namespace Jogos.API.Application.Interfaces
{
    public interface IDesenvolvedoraUseCase
    {
        Task<PageResultModel<IEnumerable<DesenvolvedoraEntity>>> ObterTodosDesenvolvedorasAsync(int Deslocamento, int RegistroRetornado);
        DesenvolvedoraEntity? ObterUmaDesenvolvedora(int Id);
        DesenvolvedoraEntity? AdicionarDesenvolvedora(DesenvolvedoraRequestDto entity);
        DesenvolvedoraEntity? EditarDesenvolvedora(int Id, DesenvolvedoraRequestDto entity);
        DesenvolvedoraEntity? DeletarDesenvolvedora(int Id);
    }
}
