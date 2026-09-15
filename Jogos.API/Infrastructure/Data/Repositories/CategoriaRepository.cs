using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Exceptions;
using Jogos.API.Domain.Interfaces;
using Jogos.API.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Jogos.API.Infrastructure.Data.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly ApplicationContext _context;

        public CategoriaRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<CategoriaEntity?> AdicionarAsync(CategoriaEntity entity)
        {
            try
            {
                var existe = await _context.Categoria.CountAsync(x => x.Nome.ToUpper() == entity.Nome.ToUpper()) > 0;

                if (existe)
                    return null;

                _context.Categoria.Add(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<CategoriaEntity?> DeletarAsync(int Id)
        {
            try
            {
                var categoria = await _context.Categoria.FirstOrDefaultAsync(x => x.Id == Id);

                if (categoria is null)
                    throw new EntidadeNaoEncontradaException($"Categoria não encontrada para o id: {Id}.");

                _context.Categoria.Remove(categoria);
                await _context.SaveChangesAsync();

                return categoria;
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

        public async Task<CategoriaEntity?> EditarAsync(int Id, CategoriaEntity entity)
        {
            try
            {
                var categoria = await _context.Categoria.FirstOrDefaultAsync(x => x.Id == Id);

                if (categoria is null)
                    throw new EntidadeNaoEncontradaException($"Categoria não encontrada para o id: {Id}.");

                var existe = await _context.Categoria.CountAsync(x => x.Nome.ToUpper() == entity.Nome.ToUpper() && x.Id != Id) > 0;

                if (existe)
                    throw new NomeDuplicadoException($"Já existe uma categoria com o nome '{entity.Nome}'.");

                categoria.Nome = entity.Nome;

                _context.Categoria.Update(categoria);
                await _context.SaveChangesAsync();

                return categoria;
            }
            catch (EntidadeNaoEncontradaException)
            {
                throw;
            }
            catch (NomeDuplicadoException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<PageResultModel<IEnumerable<CategoriaEntity>>> ObterTodosAsync(int Deslocamento = 0, int RegistroRetornado = 50)
        {
            try
            {
                if (Deslocamento < 0) Deslocamento = 0;
                if (RegistroRetornado <= 0) RegistroRetornado = 50;

                var totalRegistros = await _context.Categoria.CountAsync();

                var resultado = await _context
                    .Categoria
                    .OrderBy(x => x.Id)
                    .Skip(Deslocamento)
                    .Take(RegistroRetornado)
                    .ToListAsync();

                return new PageResultModel<IEnumerable<CategoriaEntity>>
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

        public async Task<CategoriaEntity?> ObterUmAsync(int Id)
        {
            try
            {
                var categoria = await _context
                    .Categoria
                    .FirstOrDefaultAsync(x => x.Id == Id);

                if (categoria is null)
                    throw new EntidadeNaoEncontradaException($"Categoria não encontrada para o id: {Id}.");

                return categoria;
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
    }
}
