using Jogos.API.Domain.Entities;
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

        public CategoriaEntity? Adicionar(CategoriaEntity entity)
        {
            try
            {
                _context.Categoria.Add(entity);
                _context.SaveChanges();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public CategoriaEntity? Deletar(int Id)
        {
            try
            {
                var categoria = _context.Categoria.FirstOrDefault(x => x.Id == Id);

                if (categoria is null)
                    return null;

                _context.Categoria.Remove(categoria);
                _context.SaveChanges();

                return categoria;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public CategoriaEntity? Editar(int Id, CategoriaEntity entity)
        {
            try
            {
                var categoria = _context.Categoria.FirstOrDefault(x => x.Id == Id);

                if (categoria is null)
                    return null;

                categoria.Nome = entity.Nome;

                _context.Categoria.Update(categoria);
                _context.SaveChanges();

                return categoria;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<PageResultModel<IEnumerable<CategoriaEntity>>> ObterTodosAsync(int Deslocamento = 0, int RegistroRetornado = 30)
        {
            try
            {
                if (Deslocamento < 0) Deslocamento = 0;
                if (RegistroRetornado <= 0) RegistroRetornado = 30;

                var totalRegistros = await _context.Categoria.CountAsync();

                var resultado = await _context
                    .Categoria
                    .Include(x => x.Jogos)
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

        public CategoriaEntity? ObterUm(int Id)
        {
            try
            {
                var categoria = _context
                    .Categoria
                    .Include(x => x.Jogos)
                    .FirstOrDefault(x => x.Id == Id);

                if (categoria is null)
                    return null;

                return categoria;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
