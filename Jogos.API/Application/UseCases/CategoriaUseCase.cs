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

        public async Task<CategoriaEntity?> AdicionarCategoriaAsync(CategoriaRequestDto entity)
        {
            return await _categoriaRepository.AdicionarAsync(entity.ToCategoriaEntity());
        }

        public async Task<CategoriaEntity?> DeletarCategoriaAsync(int Id)
        {
            return await _categoriaRepository.DeletarAsync(Id);
        }

        public async Task<CategoriaEntity?> EditarCategoriaAsync(int Id, CategoriaRequestDto entity)
        {
            return await _categoriaRepository.EditarAsync(Id, entity.ToCategoriaEntity());
        }

        public async Task<PageResultModel<IEnumerable<CategoriaEntity>>> ObterTodosCategoriasAsync(int Deslocamento, int RegistroRetornado)
        {
            return await _categoriaRepository.ObterTodosAsync(Deslocamento, RegistroRetornado);
        }

        public async Task<CategoriaEntity?> ObterUmaCategoriaAsync(int Id)
        {
            return await _categoriaRepository.ObterUmAsync(Id);
        }
    }
}
