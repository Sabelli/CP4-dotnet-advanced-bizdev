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

        public DesenvolvedoraEntity? AdicionarDesenvolvedora(DesenvolvedoraRequestDto entity)
        {
            return _desenvolvedoraRepository.Adicionar(entity.ToDesenvolvedoraEntity());
        }

        public DesenvolvedoraEntity? DeletarDesenvolvedora(int Id)
        {
            return _desenvolvedoraRepository.Deletar(Id);
        }

        public DesenvolvedoraEntity? EditarDesenvolvedora(int Id, DesenvolvedoraRequestDto entity)
        {
            return _desenvolvedoraRepository.Editar(Id, entity.ToDesenvolvedoraEntity());
        }

        public async Task<PageResultModel<IEnumerable<DesenvolvedoraEntity>>> ObterTodosDesenvolvedorasAsync(int Deslocamento, int RegistroRetornado)
        {
            return await _desenvolvedoraRepository.ObterTodosAsync(Deslocamento, RegistroRetornado);
        }

        public DesenvolvedoraEntity? ObterUmaDesenvolvedora(int Id)
        {
            return _desenvolvedoraRepository.ObterUm(Id);
        }
    }
}
