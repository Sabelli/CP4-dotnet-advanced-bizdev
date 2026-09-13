using Jogos.API.Application.Dtos;
using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Models;

namespace Jogos.API.Application.Interfaces
{
    public interface IPlataformaUseCase
    {
        Task<PageResultModel<IEnumerable<PlataformaEntity>>> ObterTodosPlataformasAsync(int Deslocamento, int RegistroRetornado);
        Task<PlataformaEntity?> ObterUmaPlataformaAsync(int Id);
        Task<PlataformaEntity?> AdicionarPlataformaAsync(PlataformaRequestDto entity);
        Task<PlataformaEntity?> EditarPlataformaAsync(int Id, PlataformaRequestDto entity);
        Task<PlataformaEntity?> DeletarPlataformaAsync(int Id);
    }
}
