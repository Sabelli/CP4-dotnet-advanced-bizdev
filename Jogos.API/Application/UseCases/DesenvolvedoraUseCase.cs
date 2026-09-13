using Jogos.API.Application.Dtos;
using Jogos.API.Application.Interfaces;
using Jogos.API.Application.Mappers;
using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Interfaces;
using Jogos.API.Domain.Models;

namespace Jogos.API.Application.UseCases
{
    public class DesenvolvedoraUseCase : IDesenvolvedoraUseCase
    {
        private readonly IDesenvolvedoraRepository _desenvolvedoraRepository;

        public DesenvolvedoraUseCase(IDesenvolvedoraRepository desenvolvedoraRepository)
        {
            _desenvolvedoraRepository = desenvolvedoraRepository;
        }

        public async Task<DesenvolvedoraEntity?> AdicionarDesenvolvedoraAsync(DesenvolvedoraRequestDto entity)
        {
            return await _desenvolvedoraRepository.AdicionarAsync(entity.ToDesenvolvedoraEntity());
        }

        public async Task<DesenvolvedoraEntity?> DeletarDesenvolvedoraAsync(int Id)
        {
            return await _desenvolvedoraRepository.DeletarAsync(Id);
        }

        public async Task<DesenvolvedoraEntity?> EditarDesenvolvedoraAsync(int Id, DesenvolvedoraRequestDto entity)
        {
            return await _desenvolvedoraRepository.EditarAsync(Id, entity.ToDesenvolvedoraEntity());
        }

        public async Task<PageResultModel<IEnumerable<DesenvolvedoraEntity>>> ObterTodosDesenvolvedorasAsync(int Deslocamento, int RegistroRetornado)
        {
            return await _desenvolvedoraRepository.ObterTodosAsync(Deslocamento, RegistroRetornado);
        }

        public async Task<DesenvolvedoraEntity?> ObterUmaDesenvolvedoraAsync(int Id)
        {
            return await _desenvolvedoraRepository.ObterUmAsync(Id);
        }
    }
}
