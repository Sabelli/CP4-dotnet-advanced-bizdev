using Jogos.API.Application.Dtos;
using Jogos.API.Application.Interfaces;
using Jogos.API.Application.Mappers;
using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Interfaces;
using Jogos.API.Domain.Models;

namespace Jogos.API.Application.UseCases
{
    public class CategoriaUseCase : ICategoriaUseCase
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaUseCase(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public CategoriaEntity? AdicionarCategoria(CategoriaRequestDto entity)
        {
            return _categoriaRepository.Adicionar(entity.ToCategoriaEntity());
        }

        public CategoriaEntity? DeletarCategoria(int Id)
        {
            return _categoriaRepository.Deletar(Id);
        }

        public CategoriaEntity? EditarCategoria(int Id, CategoriaRequestDto entity)
        {
            return _categoriaRepository.Editar(Id, entity.ToCategoriaEntity());
        }

        public async Task<PageResultModel<IEnumerable<CategoriaEntity>>> ObterTodosCategoriasAsync(int Deslocamento, int RegistroRetornado)
        {
            return await _categoriaRepository.ObterTodosAsync(Deslocamento, RegistroRetornado);
        }

        public CategoriaEntity? ObterUmaCategoria(int Id)
        {
            return _categoriaRepository.ObterUm(Id);
        }
    }
}
