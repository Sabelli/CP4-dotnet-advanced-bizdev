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

        public async Task<JogoEntity?> AdicionarAsync(JogoEntity entity, IEnumerable<int>? categoriaIds, IEnumerable<int>? plataformaIds)
        {
            try
            {
                var existe = await _context.Jogo.CountAsync(x => x.Nome.ToUpper() == entity.Nome.ToUpper()) > 0;

                if (existe)
                    return null;

                if (categoriaIds is not null && categoriaIds.Any())
                    entity.Categorias = await _context.Categoria.Where(x => categoriaIds.Contains(x.Id)).ToListAsync();

                if (plataformaIds is not null && plataformaIds.Any())
                    entity.Plataformas = await _context.Plataforma.Where(x => plataformaIds.Contains(x.Id)).ToListAsync();

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
                    .Include(x => x.Plataformas)
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
                    .Include(x => x.Plataformas)
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
                    .Include(x => x.Plataformas)
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
                    .Include(x => x.Plataformas)
                    .Where(x => x.Plataformas!.Any(p => p.Nome.ToUpper() == plataforma.ToUpper()))
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
                    .Include(x => x.Plataformas)
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
                    .Include(x => x.Plataformas)
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

        public async Task<JogoEntity?> DesvincularCategoriaAsync(int idJogo, int idCategoria)
        {
            try
            {
                var jogo = await _context.Jogo.Include(x => x.Categorias).FirstOrDefaultAsync(x => x.Id == idJogo);

                if (jogo is null)
                    return null;

                var categoria = jogo.Categorias?.FirstOrDefault(x => x.Id == idCategoria);

                if (categoria is not null)
                    jogo.Categorias!.Remove(categoria);

                await _context.SaveChangesAsync();

                return jogo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<JogoEntity?> VincularPlataformaAsync(int idJogo, int idPlataforma)
        {
            try
            {
                var jogo = await _context.Jogo.Include(x => x.Plataformas).FirstOrDefaultAsync(x => x.Id == idJogo);
                var plataforma = await _context.Plataforma.FirstOrDefaultAsync(x => x.Id == idPlataforma);

                if (jogo is null || plataforma is null)
                    return null;

                jogo.Plataformas ??= [];

                if (!jogo.Plataformas.Any(x => x.Id == idPlataforma))
                    jogo.Plataformas.Add(plataforma);

                await _context.SaveChangesAsync();

                return jogo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<JogoEntity?> DesvincularPlataformaAsync(int idJogo, int idPlataforma)
        {
            try
            {
                var jogo = await _context.Jogo.Include(x => x.Plataformas).FirstOrDefaultAsync(x => x.Id == idJogo);

                if (jogo is null)
                    return null;

                var plataforma = jogo.Plataformas?.FirstOrDefault(x => x.Id == idPlataforma);

                if (plataforma is not null)
                    jogo.Plataformas!.Remove(plataforma);

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
