using Jogos.API.Application.Dtos;
using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Models;

namespace Jogos.API.Application.Interfaces
{
    public interface IDesenvolvedoraUseCase
    {
        Task<PageResultModel<IEnumerable<DesenvolvedoraEntity>>> ObterTodosDesenvolvedorasAsync(int Deslocamento, int RegistroRetornado);
        Task<DesenvolvedoraEntity?> ObterUmaDesenvolvedoraAsync(int Id);
        Task<DesenvolvedoraEntity?> AdicionarDesenvolvedoraAsync(DesenvolvedoraRequestDto entity);
        Task<DesenvolvedoraEntity?> EditarDesenvolvedoraAsync(int Id, DesenvolvedoraRequestDto entity);
        Task<DesenvolvedoraEntity?> DeletarDesenvolvedoraAsync(int Id);
    }
}
