using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Interfaces;
using Jogos.API.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Jogos.API.Infrastructure.Data.Repositories
{
    public class JogoRepository : IJogoRepository
    {
        private readonly ApplicationContext _context;

        public JogoRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<JogoEntity?> AdicionarAsync(JogoEntity entity)
        {
            try
            {
                _context.Jogo.Add(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<JogoEntity?> DeletarAsync(int Id)
        {
            try
            {
                var jogo = await _context.Jogo.FirstOrDefaultAsync(x => x.Id == Id);

                if (jogo is null)
                    return null;

                _context.Jogo.Remove(jogo);
                await _context.SaveChangesAsync();

                return jogo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<JogoEntity?> EditarAsync(int Id, JogoEntity entity)
        {
            try
            {
                var jogo = await _context.Jogo.FirstOrDefaultAsync(x => x.Id == Id);

                if (jogo is null)
                    return null;

                jogo.Nome = entity.Nome;
                jogo.Preco = entity.Preco;
                jogo.Plataforma = entity.Plataforma;
                jogo.DataLancamento = entity.DataLancamento;
                jogo.DesenvolvedoraId = entity.DesenvolvedoraId;

                _context.Jogo.Update(jogo);
                await _context.SaveChangesAsync();

                return jogo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<PageResultModel<IEnumerable<JogoEntity>>> ObterTodosAsync(int Deslocamento = 0, int RegistroRetornado = 50)
        {
            try
            {
                if (Deslocamento < 0) Deslocamento = 0;
                if (RegistroRetornado <= 0) RegistroRetornado = 50;

                var totalRegistros = await _context.Jogo.CountAsync();

                var resultado = await _context
                    .Jogo
                    .Include(x => x.Desenvolvedora)
                    .Include(x => x.Categorias)
                    .OrderBy(x => x.Id)
                    .Skip(Deslocamento)
                    .Take(RegistroRetornado)
                    .ToListAsync();

                return new PageResultModel<IEnumerable<JogoEntity>>
                {
                    Data = resultado,
                    Deslocamento = Deslocamento,
                    RegistroRetornado = RegistroRetornado,
                    TotalRegistros = totalRegistros
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<JogoEntity?> ObterUmAsync(int Id)
        {
            try
            {
                return await _context
                    .Jogo
                    .Include(x => x.Desenvolvedora)
                    .Include(x => x.Categorias)
                    .FirstOrDefaultAsync(x => x.Id == Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<JogoEntity>> ObterPorNomeAsync(string nome, int Deslocamento = 0, int RegistroRetornado = 50)
        {
            try
            {
                if (Deslocamento < 0) Deslocamento = 0;
                if (RegistroRetornado <= 0) RegistroRetornado = 50;

                return await _context
                    .Jogo
                    .Include(x => x.Desenvolvedora)
                    .Include(x => x.Categorias)
                    .Where(x => x.Nome.Contains(nome))
                    .OrderBy(x => x.Id)
                    .Skip(Deslocamento)
                    .Take(RegistroRetornado)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<JogoEntity>> ObterPorPlataformaAsync(string plataforma, int Deslocamento = 0, int RegistroRetornado = 50)
        {
            try
            {
                if (Deslocamento < 0) Deslocamento = 0;
                if (RegistroRetornado <= 0) RegistroRetornado = 50;

                return await _context
                    .Jogo
                    .Include(x => x.Desenvolvedora)
                    .Include(x => x.Categorias)
                    .Where(x => x.Plataforma == plataforma)
                    .OrderBy(x => x.Id)
                    .Skip(Deslocamento)
                    .Take(RegistroRetornado)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<JogoEntity>> ObterPorDesenvolvedoraAsync(int idDesenvolvedora, int Deslocamento = 0, int RegistroRetornado = 50)
        {
            try
            {
                if (Deslocamento < 0) Deslocamento = 0;
                if (RegistroRetornado <= 0) RegistroRetornado = 50;

                return await _context
                    .Jogo
                    .Include(x => x.Desenvolvedora)
                    .Include(x => x.Categorias)
                    .Where(x => x.DesenvolvedoraId == idDesenvolvedora)
                    .OrderBy(x => x.Id)
                    .Skip(Deslocamento)
                    .Take(RegistroRetornado)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<JogoEntity>> ObterPorCategoriaAsync(int idCategoria, int Deslocamento = 0, int RegistroRetornado = 50)
        {
            try
            {
                if (Deslocamento < 0) Deslocamento = 0;
                if (RegistroRetornado <= 0) RegistroRetornado = 50;

                return await _context
                    .Jogo
                    .Include(x => x.Desenvolvedora)
                    .Include(x => x.Categorias)
                    .Where(x => x.Categorias!.Any(c => c.Id == idCategoria))
                    .OrderBy(x => x.Id)
                    .Skip(Deslocamento)
                    .Take(RegistroRetornado)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<JogoEntity?> VincularCategoriaAsync(int idJogo, int idCategoria)
        {
            try
            {
                var jogo = await _context.Jogo.Include(x => x.Categorias).FirstOrDefaultAsync(x => x.Id == idJogo);
                var categoria = await _context.Categoria.FirstOrDefaultAsync(x => x.Id == idCategoria);

                if (jogo is null || categoria is null)
                    return null;

                jogo.Categorias ??= [];

                if (!jogo.Categorias.Any(x => x.Id == idCategoria))
                    jogo.Categorias.Add(categoria);

                await _context.SaveChangesAsync();

                return jogo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
