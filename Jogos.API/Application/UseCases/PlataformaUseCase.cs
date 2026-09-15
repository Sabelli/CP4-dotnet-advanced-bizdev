using Jogos.API.Application.Dtos;
using Jogos.API.Application.Interfaces;
using Jogos.API.Application.Mappers;
using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Interfaces;
using Jogos.API.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Jogos.API.Application.UseCases
{
    public class PlataformaUseCase : IPlataformaUseCase
    {
        private readonly IPlataformaRepository _plataformaRepository;
        private readonly ILogger<PlataformaUseCase> _logger;

        public PlataformaUseCase(IPlataformaRepository plataformaRepository, ILogger<PlataformaUseCase> logger)
        {
            _plataformaRepository = plataformaRepository;
            _logger = logger;
        }

        public async Task<PlataformaEntity?> AdicionarPlataformaAsync(PlataformaRequestDto entity)
        {
            try
            {
                _logger.LogInformation("Adicionando plataforma {Nome}", entity.Nome);

                return await _plataformaRepository.AdicionarAsync(entity.ToPlataformaEntity());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao adicionar a plataforma {Nome}", entity.Nome);
                throw;
            }
        }

        public async Task<PlataformaEntity?> DeletarPlataformaAsync(int Id)
        {
            try
            {
                _logger.LogInformation("Deletando plataforma {PlataformaId}", Id);

                return await _plataformaRepository.DeletarAsync(Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao deletar a plataforma {PlataformaId}", Id);
                throw;
            }
        }

        public async Task<PlataformaEntity?> EditarPlataformaAsync(int Id, PlataformaRequestDto entity)
        {
            try
            {
                _logger.LogInformation("Editando plataforma {PlataformaId}", Id);

                return await _plataformaRepository.EditarAsync(Id, entity.ToPlataformaEntity());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao editar a plataforma {PlataformaId}", Id);
                throw;
            }
        }

        public async Task<PageResultModel<IEnumerable<PlataformaEntity>>> ObterTodosPlataformasAsync(int Deslocamento, int RegistroRetornado)
        {
            try
            {
                _logger.LogInformation("Obtendo plataformas, Deslocamento={Deslocamento}, RegistroRetornado={RegistroRetornado}", Deslocamento, RegistroRetornado);

                return await _plataformaRepository.ObterTodosAsync(Deslocamento, RegistroRetornado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao obter as plataformas");
                throw;
            }
        }

        public async Task<PlataformaEntity?> ObterUmaPlataformaAsync(int Id)
        {
            try
            {
                _logger.LogInformation("Obtendo plataforma {PlataformaId}", Id);

                return await _plataformaRepository.ObterUmAsync(Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao obter a plataforma {PlataformaId}", Id);
                throw;
            }
        }
    }
}
