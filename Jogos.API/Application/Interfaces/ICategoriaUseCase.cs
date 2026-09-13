using Jogos.API.Application.Dtos;
using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Models;

namespace Jogos.API.Application.Interfaces
{
    public interface ICategoriaUseCase
    {
        Task<PageResultModel<IEnumerable<CategoriaEntity>>> ObterTodosCategoriasAsync(int Deslocamento, int RegistroRetornado);
        Task<CategoriaEntity?> ObterUmaCategoriaAsync(int Id);
        Task<CategoriaEntity?> AdicionarCategoriaAsync(CategoriaRequestDto entity);
        Task<CategoriaEntity?> EditarCategoriaAsync(int Id, CategoriaRequestDto entity);
        Task<CategoriaEntity?> DeletarCategoriaAsync(int Id);
    }
}
