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

        public async Task<JogoEntity?> AdicionarJogoAsync(JogoRequestDto entity)
        {
            return await _jogoRepository.AdicionarAsync(entity.ToJogoEntity());
        }

        public async Task<JogoEntity?> DeletarJogoAsync(int Id)
        {
            return await _jogoRepository.DeletarAsync(Id);
        }

        public async Task<JogoEntity?> EditarJogoAsync(int Id, JogoRequestDto entity)
        {
            return await _jogoRepository.EditarAsync(Id, entity.ToJogoEntity());
        }

        public async Task<PageResultModel<IEnumerable<JogoEntity>>> ObterTodosJogosAsync(int Deslocamento, int RegistroRetornado)
        {
            return await _jogoRepository.ObterTodosAsync(Deslocamento, RegistroRetornado);
        }

        public async Task<JogoEntity?> ObterUmJogoAsync(int Id)
        {
            return await _jogoRepository.ObterUmAsync(Id);
        }

        public async Task<IEnumerable<JogoEntity>> ObterJogosPorNomeAsync(string nome, int Deslocamento, int RegistroRetornado)
        {
            return await _jogoRepository.ObterPorNomeAsync(nome, Deslocamento, RegistroRetornado);
        }

        public async Task<IEnumerable<JogoEntity>> ObterJogosPorPlataformaAsync(string plataforma, int Deslocamento, int RegistroRetornado)
        {
            return await _jogoRepository.ObterPorPlataformaAsync(plataforma, Deslocamento, RegistroRetornado);
        }

        public async Task<IEnumerable<JogoEntity>> ObterJogosPorDesenvolvedoraAsync(int idDesenvolvedora, int Deslocamento, int RegistroRetornado)
        {
            return await _jogoRepository.ObterPorDesenvolvedoraAsync(idDesenvolvedora, Deslocamento, RegistroRetornado);
        }

        public async Task<IEnumerable<JogoEntity>> ObterJogosPorCategoriaAsync(int idCategoria, int Deslocamento, int RegistroRetornado)
        {
            return await _jogoRepository.ObterPorCategoriaAsync(idCategoria, Deslocamento, RegistroRetornado);
        }

        public async Task<JogoEntity?> VincularCategoriaAsync(int idJogo, int idCategoria)
        {
            return await _jogoRepository.VincularCategoriaAsync(idJogo, idCategoria);
        }
    }
}
