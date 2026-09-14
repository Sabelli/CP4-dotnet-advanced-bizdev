using Jogos.API.Application.Dtos;
using Jogos.API.Application.Interfaces;
using Jogos.API.Application.Mappers;
using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Interfaces;
using Jogos.API.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Jogos.API.Application.UseCases
{
    public class JogoUseCase : IJogoUseCase
    {
        private readonly IJogoRepository _jogoRepository;
        private readonly ILogger<JogoUseCase> _logger;

        public JogoUseCase(IJogoRepository jogoRepository, ILogger<JogoUseCase> logger)
        {
            _jogoRepository = jogoRepository;
            _logger = logger;
        }

        public async Task<JogoEntity?> AdicionarJogoAsync(JogoRequestDto entity)
        {
            try
            {
                _logger.LogInformation("Adicionando jogo {Nome}", entity.Nome);

                return await _jogoRepository.AdicionarAsync(entity.ToJogoEntity(), entity.CategoriaIds, entity.PlataformaIds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao adicionar o jogo {Nome}", entity.Nome);
                throw;
            }
        }

        public async Task<JogoEntity?> DeletarJogoAsync(int Id)
        {
            try
            {
                _logger.LogInformation("Deletando jogo {JogoId}", Id);

                var jogo = await _jogoRepository.DeletarAsync(Id);

                if (jogo is null)
                    _logger.LogWarning("Jogo {JogoId} não encontrado para deleção", Id);

                return jogo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao deletar o jogo {JogoId}", Id);
                throw;
            }
        }

        public async Task<JogoEntity?> EditarJogoAsync(int Id, JogoUpdateRequestDto entity)
        {
            try
            {
                _logger.LogInformation("Editando jogo {JogoId}", Id);

                var jogo = await _jogoRepository.EditarAsync(Id, entity.ToJogoEntity());

                if (jogo is null)
                    _logger.LogWarning("Jogo {JogoId} não encontrado para edição", Id);

                return jogo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao editar o jogo {JogoId}", Id);
                throw;
            }
        }

        public async Task<PageResultModel<IEnumerable<JogoEntity>>> ObterTodosJogosAsync(int Deslocamento, int RegistroRetornado)
        {
            try
            {
                _logger.LogInformation("Obtendo jogos, Deslocamento={Deslocamento}, RegistroRetornado={RegistroRetornado}", Deslocamento, RegistroRetornado);

                return await _jogoRepository.ObterTodosAsync(Deslocamento, RegistroRetornado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao obter os jogos");
                throw;
            }
        }

        public async Task<JogoEntity?> ObterUmJogoAsync(int Id)
        {
            try
            {
                _logger.LogInformation("Obtendo jogo {JogoId}", Id);

                var jogo = await _jogoRepository.ObterUmAsync(Id);

                if (jogo is null)
                    _logger.LogWarning("Jogo {JogoId} não encontrado", Id);

                return jogo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao obter o jogo {JogoId}", Id);
                throw;
            }
        }

        public async Task<IEnumerable<JogoEntity>> ObterJogosPorNomeAsync(string nome, int Deslocamento, int RegistroRetornado)
        {
            try
            {
                _logger.LogInformation("Obtendo jogos pelo nome {Nome}", nome);

                return await _jogoRepository.ObterPorNomeAsync(nome, Deslocamento, RegistroRetornado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao obter jogos pelo nome {Nome}", nome);
                throw;
            }
        }

        public async Task<IEnumerable<JogoEntity>> ObterJogosPorPlataformaAsync(string plataforma, int Deslocamento, int RegistroRetornado)
        {
            try
            {
                _logger.LogInformation("Obtendo jogos pela plataforma {Plataforma}", plataforma);

                return await _jogoRepository.ObterPorPlataformaAsync(plataforma, Deslocamento, RegistroRetornado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao obter jogos pela plataforma {Plataforma}", plataforma);
                throw;
            }
        }

        public async Task<IEnumerable<JogoEntity>> ObterJogosPorDesenvolvedoraAsync(int idDesenvolvedora, int Deslocamento, int RegistroRetornado)
        {
            try
            {
                _logger.LogInformation("Obtendo jogos pela desenvolvedora {DesenvolvedoraId}", idDesenvolvedora);

                return await _jogoRepository.ObterPorDesenvolvedoraAsync(idDesenvolvedora, Deslocamento, RegistroRetornado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao obter jogos pela desenvolvedora {DesenvolvedoraId}", idDesenvolvedora);
                throw;
            }
        }

        public async Task<IEnumerable<JogoEntity>> ObterJogosPorCategoriaAsync(int idCategoria, int Deslocamento, int RegistroRetornado)
        {
            try
            {
                _logger.LogInformation("Obtendo jogos pela categoria {CategoriaId}", idCategoria);

                return await _jogoRepository.ObterPorCategoriaAsync(idCategoria, Deslocamento, RegistroRetornado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao obter jogos pela categoria {CategoriaId}", idCategoria);
                throw;
            }
        }

        public async Task<JogoEntity?> VincularCategoriaAsync(int idJogo, IEnumerable<int> categoriaIds)
        {
            try
            {
                _logger.LogInformation("Vinculando categorias {CategoriaIds} ao jogo {JogoId}", categoriaIds, idJogo);

                var jogo = await _jogoRepository.VincularCategoriaAsync(idJogo, categoriaIds);

                if (jogo is null)
                    _logger.LogWarning("Jogo {JogoId} não encontrado para vínculo", idJogo);

                return jogo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao vincular categorias {CategoriaIds} ao jogo {JogoId}", categoriaIds, idJogo);
                throw;
            }
        }

        public async Task<JogoEntity?> DesvincularCategoriaAsync(int idJogo, IEnumerable<int> categoriaIds)
        {
            try
            {
                _logger.LogInformation("Desvinculando categorias {CategoriaIds} do jogo {JogoId}", categoriaIds, idJogo);

                var jogo = await _jogoRepository.DesvincularCategoriaAsync(idJogo, categoriaIds);

                if (jogo is null)
                    _logger.LogWarning("Jogo {JogoId} não encontrado para desvínculo", idJogo);

                return jogo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao desvincular categorias {CategoriaIds} do jogo {JogoId}", categoriaIds, idJogo);
                throw;
            }
        }

        public async Task<JogoEntity?> VincularPlataformaAsync(int idJogo, IEnumerable<int> plataformaIds)
        {
            try
            {
                _logger.LogInformation("Vinculando plataformas {PlataformaIds} ao jogo {JogoId}", plataformaIds, idJogo);

                var jogo = await _jogoRepository.VincularPlataformaAsync(idJogo, plataformaIds);

                if (jogo is null)
                    _logger.LogWarning("Jogo {JogoId} não encontrado para vínculo", idJogo);

                return jogo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao vincular plataformas {PlataformaIds} ao jogo {JogoId}", plataformaIds, idJogo);
                throw;
            }
        }

        public async Task<JogoEntity?> DesvincularPlataformaAsync(int idJogo, IEnumerable<int> plataformaIds)
        {
            try
            {
                _logger.LogInformation("Desvinculando plataformas {PlataformaIds} do jogo {JogoId}", plataformaIds, idJogo);

                var jogo = await _jogoRepository.DesvincularPlataformaAsync(idJogo, plataformaIds);

                if (jogo is null)
                    _logger.LogWarning("Jogo {JogoId} não encontrado para desvínculo", idJogo);

                return jogo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao desvincular plataformas {PlataformaIds} do jogo {JogoId}", plataformaIds, idJogo);
                throw;
            }
        }
    }
}
