using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Exceptions;
using Jogos.API.Domain.Interfaces;
using Jogos.API.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Jogos.API.Infrastructure.Data.Repositories
{
    public class DesenvolvedoraRepository : IDesenvolvedoraRepository
    {
        private readonly ApplicationContext _context;

        public DesenvolvedoraRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<DesenvolvedoraEntity?> AdicionarAsync(DesenvolvedoraEntity entity)
        {
            try
            {
                var existe = await _context.Desenvolvedora.CountAsync(x => x.Nome.ToUpper() == entity.Nome.ToUpper()) > 0;

                if (existe)
                    return null;

                _context.Desenvolvedora.Add(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<DesenvolvedoraEntity?> DeletarAsync(int Id)
        {
            try
            {
                var desenvolvedora = await _context.Desenvolvedora.FirstOrDefaultAsync(x => x.Id == Id);

                if (desenvolvedora is null)
                    throw new EntidadeNaoEncontradaException($"Desenvolvedora não encontrada para o id: {Id}.");

                _context.Desenvolvedora.Remove(desenvolvedora);
                await _context.SaveChangesAsync();

                return desenvolvedora;
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

        public async Task<DesenvolvedoraEntity?> EditarAsync(int Id, DesenvolvedoraEntity entity)
        {
            try
            {
                var desenvolvedora = await _context.Desenvolvedora.FirstOrDefaultAsync(x => x.Id == Id);

                if (desenvolvedora is null)
                    throw new EntidadeNaoEncontradaException($"Desenvolvedora não encontrada para o id: {Id}.");

                var existe = await _context.Desenvolvedora.CountAsync(x => x.Nome.ToUpper() == entity.Nome.ToUpper() && x.Id != Id) > 0;

                if (existe)
                    throw new NomeDuplicadoException($"Já existe uma desenvolvedora com o nome '{entity.Nome}'.");

                desenvolvedora.Nome = entity.Nome;

                _context.Desenvolvedora.Update(desenvolvedora);
                await _context.SaveChangesAsync();

                return desenvolvedora;
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

        public async Task<PageResultModel<IEnumerable<DesenvolvedoraEntity>>> ObterTodosAsync(int Deslocamento = 0, int RegistroRetornado = 50)
        {
            try
            {
                if (Deslocamento < 0) Deslocamento = 0;
                if (RegistroRetornado <= 0) RegistroRetornado = 50;

                var totalRegistros = await _context.Desenvolvedora.CountAsync();

                var resultado = await _context
                    .Desenvolvedora
                    .OrderBy(x => x.Id)
                    .Skip(Deslocamento)
                    .Take(RegistroRetornado)
                    .ToListAsync();

                return new PageResultModel<IEnumerable<DesenvolvedoraEntity>>
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

        public async Task<DesenvolvedoraEntity?> ObterUmAsync(int Id)
        {
            try
            {
                var desenvolvedora = await _context
                    .Desenvolvedora
                    .FirstOrDefaultAsync(x => x.Id == Id);

                if (desenvolvedora is null)
                    throw new EntidadeNaoEncontradaException($"Desenvolvedora não encontrada para o id: {Id}.");

                return desenvolvedora;
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
