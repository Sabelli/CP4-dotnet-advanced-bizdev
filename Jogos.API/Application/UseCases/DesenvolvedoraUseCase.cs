using Jogos.API.Application.Dtos;
using Jogos.API.Application.Interfaces;
using Jogos.API.Application.Mappers;
using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Interfaces;
using Jogos.API.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Jogos.API.Application.UseCases
{
    public class DesenvolvedoraUseCase : IDesenvolvedoraUseCase
    {
        private readonly IDesenvolvedoraRepository _desenvolvedoraRepository;
        private readonly ILogger<DesenvolvedoraUseCase> _logger;

        public DesenvolvedoraUseCase(IDesenvolvedoraRepository desenvolvedoraRepository, ILogger<DesenvolvedoraUseCase> logger)
        {
            _desenvolvedoraRepository = desenvolvedoraRepository;
            _logger = logger;
        }

        public async Task<DesenvolvedoraEntity?> AdicionarDesenvolvedoraAsync(DesenvolvedoraRequestDto entity)
        {
            try
            {
                _logger.LogInformation("Adicionando desenvolvedora {Nome}", entity.Nome);

                return await _desenvolvedoraRepository.AdicionarAsync(entity.ToDesenvolvedoraEntity());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao adicionar a desenvolvedora {Nome}", entity.Nome);
                throw;
            }
        }

        public async Task<DesenvolvedoraEntity?> DeletarDesenvolvedoraAsync(int Id)
        {
            try
            {
                _logger.LogInformation("Deletando desenvolvedora {DesenvolvedoraId}", Id);

                var desenvolvedora = await _desenvolvedoraRepository.DeletarAsync(Id);

                if (desenvolvedora is null)
                    _logger.LogWarning("Desenvolvedora {DesenvolvedoraId} não encontrada para deleção", Id);

                return desenvolvedora;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao deletar a desenvolvedora {DesenvolvedoraId}", Id);
                throw;
            }
        }

        public async Task<DesenvolvedoraEntity?> EditarDesenvolvedoraAsync(int Id, DesenvolvedoraRequestDto entity)
        {
            try
            {
                _logger.LogInformation("Editando desenvolvedora {DesenvolvedoraId}", Id);

                var desenvolvedora = await _desenvolvedoraRepository.EditarAsync(Id, entity.ToDesenvolvedoraEntity());

                if (desenvolvedora is null)
                    _logger.LogWarning("Desenvolvedora {DesenvolvedoraId} não encontrada para edição", Id);

                return desenvolvedora;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao editar a desenvolvedora {DesenvolvedoraId}", Id);
                throw;
            }
        }

        public async Task<PageResultModel<IEnumerable<DesenvolvedoraEntity>>> ObterTodosDesenvolvedorasAsync(int Deslocamento, int RegistroRetornado)
        {
            try
            {
                _logger.LogInformation("Obtendo desenvolvedoras, Deslocamento={Deslocamento}, RegistroRetornado={RegistroRetornado}", Deslocamento, RegistroRetornado);

                return await _desenvolvedoraRepository.ObterTodosAsync(Deslocamento, RegistroRetornado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao obter as desenvolvedoras");
                throw;
            }
        }

        public async Task<DesenvolvedoraEntity?> ObterUmaDesenvolvedoraAsync(int Id)
        {
            try
            {
                _logger.LogInformation("Obtendo desenvolvedora {DesenvolvedoraId}", Id);

                var desenvolvedora = await _desenvolvedoraRepository.ObterUmAsync(Id);

                if (desenvolvedora is null)
                    _logger.LogWarning("Desenvolvedora {DesenvolvedoraId} não encontrada", Id);

                return desenvolvedora;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao obter a desenvolvedora {DesenvolvedoraId}", Id);
                throw;
            }
        }
    }
}
