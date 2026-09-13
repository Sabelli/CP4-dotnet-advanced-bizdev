using Jogos.API.Application.Dtos;
using Jogos.API.Application.Interfaces;
using Jogos.API.Application.Mappers;
using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Interfaces;
using Jogos.API.Domain.Models;

namespace Jogos.API.Application.UseCases
{
    public class JogoUseCase : IJogoUseCase
    {
        private readonly IJogoRepository _jogoRepository;

        public JogoUseCase(IJogoRepository jogoRepository)
        {
            _jogoRepository = jogoRepository;
        }

        public JogoEntity? AdicionarJogo(JogoRequestDto entity)
        {
            return _jogoRepository.Adicionar(entity.ToJogoEntity());
        }

        public JogoEntity? DeletarJogo(int Id)
        {
            return _jogoRepository.Deletar(Id);
        }

        public JogoEntity? EditarJogo(int Id, JogoRequestDto entity)
        {
            return _jogoRepository.Editar(Id, entity.ToJogoEntity());
        }

        public async Task<PageResultModel<IEnumerable<JogoEntity>>> ObterTodosJogosAsync(int Deslocamento, int RegistroRetornado)
        {
            return await _jogoRepository.ObterTodosAsync(Deslocamento, RegistroRetornado);
        }

        public JogoEntity? ObterUmJogo(int Id)
        {
            return _jogoRepository.ObterUm(Id);
        }
    }
}
