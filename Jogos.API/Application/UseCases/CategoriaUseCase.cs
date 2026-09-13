using Jogos.API.Application.Dtos;
using Jogos.API.Application.Interfaces;
using Jogos.API.Application.Mappers;
using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Interfaces;
using Jogos.API.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Jogos.API.Application.UseCases
{
    public class CategoriaUseCase : ICategoriaUseCase
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ILogger<CategoriaUseCase> _logger;

        public CategoriaUseCase(ICategoriaRepository categoriaRepository, ILogger<CategoriaUseCase> logger)
        {
            _categoriaRepository = categoriaRepository;
            _logger = logger;
        }

        public async Task<CategoriaEntity?> AdicionarCategoriaAsync(CategoriaRequestDto entity)
        {
            try
            {
                _logger.LogInformation("Adicionando categoria {Nome}", entity.Nome);

                return await _categoriaRepository.AdicionarAsync(entity.ToCategoriaEntity());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao adicionar a categoria {Nome}", entity.Nome);
                throw;
            }
        }

        public async Task<CategoriaEntity?> DeletarCategoriaAsync(int Id)
        {
            try
            {
                _logger.LogInformation("Deletando categoria {CategoriaId}", Id);

                var categoria = await _categoriaRepository.DeletarAsync(Id);

                if (categoria is null)
                    _logger.LogWarning("Categoria {CategoriaId} não encontrada para deleção", Id);

                return categoria;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao deletar a categoria {CategoriaId}", Id);
                throw;
            }
        }

        public async Task<CategoriaEntity?> EditarCategoriaAsync(int Id, CategoriaRequestDto entity)
        {
            try
            {
                _logger.LogInformation("Editando categoria {CategoriaId}", Id);

                var categoria = await _categoriaRepository.EditarAsync(Id, entity.ToCategoriaEntity());

                if (categoria is null)
                    _logger.LogWarning("Categoria {CategoriaId} não encontrada para edição", Id);

                return categoria;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao editar a categoria {CategoriaId}", Id);
                throw;
            }
        }

        public async Task<PageResultModel<IEnumerable<CategoriaEntity>>> ObterTodosCategoriasAsync(int Deslocamento, int RegistroRetornado)
        {
            try
            {
                _logger.LogInformation("Obtendo categorias, Deslocamento={Deslocamento}, RegistroRetornado={RegistroRetornado}", Deslocamento, RegistroRetornado);

                return await _categoriaRepository.ObterTodosAsync(Deslocamento, RegistroRetornado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao obter as categorias");
                throw;
            }
        }

        public async Task<CategoriaEntity?> ObterUmaCategoriaAsync(int Id)
        {
            try
            {
                _logger.LogInformation("Obtendo categoria {CategoriaId}", Id);

                var categoria = await _categoriaRepository.ObterUmAsync(Id);

                if (categoria is null)
                    _logger.LogWarning("Categoria {CategoriaId} não encontrada", Id);

                return categoria;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao obter a categoria {CategoriaId}", Id);
                throw;
            }
        }
    }
}
