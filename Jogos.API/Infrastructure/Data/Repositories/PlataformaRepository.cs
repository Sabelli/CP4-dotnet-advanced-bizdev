using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Exceptions;
using Jogos.API.Domain.Interfaces;
using Jogos.API.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Jogos.API.Infrastructure.Data.Repositories
{
    public class PlataformaRepository : IPlataformaRepository
    {
        private readonly ApplicationContext _context;

        public PlataformaRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<PlataformaEntity?> AdicionarAsync(PlataformaEntity entity)
        {
            try
            {
                var existe = await _context.Plataforma.CountAsync(x => x.Nome.ToUpper() == entity.Nome.ToUpper()) > 0;

                if (existe)
                    return null;

                _context.Plataforma.Add(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<PlataformaEntity?> DeletarAsync(int Id)
        {
            try
            {
                var plataforma = await _context.Plataforma.FirstOrDefaultAsync(x => x.Id == Id);

                if (plataforma is null)
                    return null;

                _context.Plataforma.Remove(plataforma);
                await _context.SaveChangesAsync();

                return plataforma;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<PlataformaEntity?> EditarAsync(int Id, PlataformaEntity entity)
        {
            try
            {
                var plataforma = await _context.Plataforma.FirstOrDefaultAsync(x => x.Id == Id);

                if (plataforma is null)
                    return null;

                var existe = await _context.Plataforma.CountAsync(x => x.Nome.ToUpper() == entity.Nome.ToUpper() && x.Id != Id) > 0;

                if (existe)
                    throw new NomeDuplicadoException($"Já existe uma plataforma com o nome '{entity.Nome}'.");

                plataforma.Nome = entity.Nome;

                _context.Plataforma.Update(plataforma);
                await _context.SaveChangesAsync();

                return plataforma;
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

        public async Task<PageResultModel<IEnumerable<PlataformaEntity>>> ObterTodosAsync(int Deslocamento = 0, int RegistroRetornado = 50)
        {
            try
            {
                if (Deslocamento < 0) Deslocamento = 0;
                if (RegistroRetornado <= 0) RegistroRetornado = 50;

                var totalRegistros = await _context.Plataforma.CountAsync();

                var resultado = await _context
                    .Plataforma
                    .OrderBy(x => x.Id)
                    .Skip(Deslocamento)
                    .Take(RegistroRetornado)
                    .ToListAsync();

                return new PageResultModel<IEnumerable<PlataformaEntity>>
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

        public async Task<PlataformaEntity?> ObterUmAsync(int Id)
        {
            try
            {
                return await _context
                    .Plataforma
                    .FirstOrDefaultAsync(x => x.Id == Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
