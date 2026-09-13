using Jogos.API.Application.Dtos;
using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Models;

namespace Jogos.API.Application.Interfaces
{
    public interface ICategoriaUseCase
    {
        Task<PageResultModel<IEnumerable<CategoriaEntity>>> ObterTodosCategoriasAsync(int Deslocamento, int RegistroRetornado);
        CategoriaEntity? ObterUmaCategoria(int Id);
        CategoriaEntity? AdicionarCategoria(CategoriaRequestDto entity);
        CategoriaEntity? EditarCategoria(int Id, CategoriaRequestDto entity);
        CategoriaEntity? DeletarCategoria(int Id);
    }
}
