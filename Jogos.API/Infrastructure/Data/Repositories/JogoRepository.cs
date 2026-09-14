using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Exceptions;
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

        private static async Task<List<CategoriaEntity>> ResolverCategoriasOuFalharAsync(DbSet<CategoriaEntity> dbSet, IEnumerable<int> ids)
        {
            var idsList = ids.Distinct().ToList();
            var resolvidas = await dbSet.Where(x => idsList.Contains(x.Id)).ToListAsync();

            if (resolvidas.Count != idsList.Count)
            {
                var faltantes = idsList.Except(resolvidas.Select(x => x.Id));
                throw new EntidadeNaoEncontradaException($"Categoria(s) não encontrada(s) para o(s) id(s): {string.Join(", ", faltantes)}.");
            }

            return resolvidas;
        }

        private static async Task<List<PlataformaEntity>> ResolverPlataformasOuFalharAsync(DbSet<PlataformaEntity> dbSet, IEnumerable<int> ids)
        {
            var idsList = ids.Distinct().ToList();
            var resolvidas = await dbSet.Where(x => idsList.Contains(x.Id)).ToListAsync();

            if (resolvidas.Count != idsList.Count)
            {
                var faltantes = idsList.Except(resolvidas.Select(x => x.Id));
                throw new EntidadeNaoEncontradaException($"Plataforma(s) não encontrada(s) para o(s) id(s): {string.Join(", ", faltantes)}.");
            }

            return resolvidas;
        }

        public async Task<JogoEntity?> AdicionarAsync(JogoEntity entity, IEnumerable<int>? categoriaIds, IEnumerable<int>? plataformaIds)
        {
            try
            {
                var existe = await _context.Jogo.CountAsync(x => x.Nome.ToUpper() == entity.Nome.ToUpper()) > 0;

                if (existe)
                    return null;

                if (categoriaIds is not null && categoriaIds.Any())
                    entity.Categorias = await ResolverCategoriasOuFalharAsync(_context.Categoria, categoriaIds);

                if (plataformaIds is not null && plataformaIds.Any())
                    entity.Plataformas = await ResolverPlataformasOuFalharAsync(_context.Plataforma, plataformaIds);

                _context.Jogo.Add(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
            catch (EntidadeNaoEncontradaException)
            {
                throw;
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
                var jogo = await _context.Jogo
                    .Include(x => x.Desenvolvedora)
                    .Include(x => x.Categorias)
                    .Include(x => x.Plataformas)
                    .FirstOrDefaultAsync(x => x.Id == Id);

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
                var jogo = await _context.Jogo
                    .Include(x => x.Desenvolvedora)
                    .Include(x => x.Categorias)
                    .Include(x => x.Plataformas)
                    .FirstOrDefaultAsync(x => x.Id == Id);

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

        public async Task<PageResultModel<IEnumerable<JogoEntity>>> ObterPorNomeAsync(string nome, int Deslocamento = 0, int RegistroRetornado = 50)
        {
            try
            {
                if (Deslocamento < 0) Deslocamento = 0;
                if (RegistroRetornado <= 0) RegistroRetornado = 50;

                var query = _context.Jogo.Where(x => x.Nome.ToUpper().Contains(nome.ToUpper()));

                var totalRegistros = await query.CountAsync();

                var resultado = await query
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

        public async Task<PageResultModel<IEnumerable<JogoEntity>>> ObterPorPlataformaAsync(int idPlataforma, int Deslocamento = 0, int RegistroRetornado = 50)
        {
            try
            {
                if (Deslocamento < 0) Deslocamento = 0;
                if (RegistroRetornado <= 0) RegistroRetornado = 50;

                var existe = await _context.Plataforma.CountAsync(x => x.Id == idPlataforma) > 0;

                if (!existe)
                    throw new EntidadeNaoEncontradaException($"Plataforma não encontrada para o id: {idPlataforma}.");

                var query = _context.Jogo.Where(x => x.Plataformas!.Any(p => p.Id == idPlataforma));

                var totalRegistros = await query.CountAsync();

                var resultado = await query
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
            catch (EntidadeNaoEncontradaException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<PageResultModel<IEnumerable<JogoEntity>>> ObterPorDesenvolvedoraAsync(int idDesenvolvedora, int Deslocamento = 0, int RegistroRetornado = 50)
        {
            try
            {
                if (Deslocamento < 0) Deslocamento = 0;
                if (RegistroRetornado <= 0) RegistroRetornado = 50;

                var existe = await _context.Desenvolvedora.CountAsync(x => x.Id == idDesenvolvedora) > 0;

                if (!existe)
                    throw new EntidadeNaoEncontradaException($"Desenvolvedora não encontrada para o id: {idDesenvolvedora}.");

                var query = _context.Jogo.Where(x => x.DesenvolvedoraId == idDesenvolvedora);

                var totalRegistros = await query.CountAsync();

                var resultado = await query
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
            catch (EntidadeNaoEncontradaException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<PageResultModel<IEnumerable<JogoEntity>>> ObterPorCategoriaAsync(int idCategoria, int Deslocamento = 0, int RegistroRetornado = 50)
        {
            try
            {
                if (Deslocamento < 0) Deslocamento = 0;
                if (RegistroRetornado <= 0) RegistroRetornado = 50;

                var existe = await _context.Categoria.CountAsync(x => x.Id == idCategoria) > 0;

                if (!existe)
                    throw new EntidadeNaoEncontradaException($"Categoria não encontrada para o id: {idCategoria}.");

                var query = _context.Jogo.Where(x => x.Categorias!.Any(c => c.Id == idCategoria));

                var totalRegistros = await query.CountAsync();

                var resultado = await query
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
            catch (EntidadeNaoEncontradaException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<JogoEntity?> VincularCategoriaAsync(int idJogo, IEnumerable<int> categoriaIds)
        {
            try
            {
                var jogo = await _context.Jogo
                    .Include(x => x.Desenvolvedora)
                    .Include(x => x.Categorias)
                    .Include(x => x.Plataformas)
                    .FirstOrDefaultAsync(x => x.Id == idJogo);

                if (jogo is null)
                    return null;

                var categorias = await ResolverCategoriasOuFalharAsync(_context.Categoria, categoriaIds);

                jogo.Categorias ??= [];

                foreach (var categoria in categorias)
                    if (!jogo.Categorias.Any(x => x.Id == categoria.Id))
                        jogo.Categorias.Add(categoria);

                await _context.SaveChangesAsync();

                return jogo;
            }
            catch (EntidadeNaoEncontradaException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<JogoEntity?> DesvincularCategoriaAsync(int idJogo, IEnumerable<int> categoriaIds)
        {
            try
            {
                var jogo = await _context.Jogo
                    .Include(x => x.Desenvolvedora)
                    .Include(x => x.Categorias)
                    .Include(x => x.Plataformas)
                    .FirstOrDefaultAsync(x => x.Id == idJogo);

                if (jogo is null)
                    return null;

                foreach (var idCategoria in categoriaIds)
                {
                    var categoria = jogo.Categorias?.FirstOrDefault(x => x.Id == idCategoria);

                    if (categoria is not null)
                        jogo.Categorias!.Remove(categoria);
                }

                await _context.SaveChangesAsync();

                return jogo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<JogoEntity?> VincularPlataformaAsync(int idJogo, IEnumerable<int> plataformaIds)
        {
            try
            {
                var jogo = await _context.Jogo
                    .Include(x => x.Desenvolvedora)
                    .Include(x => x.Categorias)
                    .Include(x => x.Plataformas)
                    .FirstOrDefaultAsync(x => x.Id == idJogo);

                if (jogo is null)
                    return null;

                var plataformas = await ResolverPlataformasOuFalharAsync(_context.Plataforma, plataformaIds);

                jogo.Plataformas ??= [];

                foreach (var plataforma in plataformas)
                    if (!jogo.Plataformas.Any(x => x.Id == plataforma.Id))
                        jogo.Plataformas.Add(plataforma);

                await _context.SaveChangesAsync();

                return jogo;
            }
            catch (EntidadeNaoEncontradaException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<JogoEntity?> DesvincularPlataformaAsync(int idJogo, IEnumerable<int> plataformaIds)
        {
            try
            {
                var jogo = await _context.Jogo
                    .Include(x => x.Desenvolvedora)
                    .Include(x => x.Categorias)
                    .Include(x => x.Plataformas)
                    .FirstOrDefaultAsync(x => x.Id == idJogo);

                if (jogo is null)
                    return null;

                foreach (var idPlataforma in plataformaIds)
                {
                    var plataforma = jogo.Plataformas?.FirstOrDefault(x => x.Id == idPlataforma);

                    if (plataforma is not null)
                        jogo.Plataformas!.Remove(plataforma);
                }

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
