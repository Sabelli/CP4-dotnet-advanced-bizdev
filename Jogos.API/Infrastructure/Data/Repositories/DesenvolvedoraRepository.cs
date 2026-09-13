using Jogos.API.Domain.Entities;
using Jogos.API.Domain.Interfaces;
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

        public DesenvolvedoraEntity? Adicionar(DesenvolvedoraEntity entity)
        {
            try
            {
                _context.Desenvolvedora.Add(entity);
                _context.SaveChanges();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public DesenvolvedoraEntity? Deletar(int Id)
        {
            try
            {
                var desenvolvedora = _context.Desenvolvedora.FirstOrDefault(x => x.Id == Id);

                if (desenvolvedora is null)
                    return null;

                _context.Desenvolvedora.Remove(desenvolvedora);
                _context.SaveChanges();

                return desenvolvedora;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public DesenvolvedoraEntity? Editar(int Id, DesenvolvedoraEntity entity)
        {
            try
            {
                var desenvolvedora = _context.Desenvolvedora.FirstOrDefault(x => x.Id == Id);

                if (desenvolvedora is null)
                    return null;

                desenvolvedora.Nome = entity.Nome;

                _context.Desenvolvedora.Update(desenvolvedora);
                _context.SaveChanges();

                return desenvolvedora;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<DesenvolvedoraEntity>> ObterTodosAsync(int Deslocamento = 0, int RegistroRetornado = 30)
        {
            try
            {
                if (Deslocamento < 0) Deslocamento = 0;
                if (RegistroRetornado <= 0) RegistroRetornado = 30;

                var resultado = await _context
                    .Desenvolvedora
                    .Include(x => x.Jogos)
                    .OrderBy(x => x.Id)
                    .Skip(Deslocamento)
                    .Take(RegistroRetornado)
                    .ToListAsync();

                if (!resultado.Any())
                    return Enumerable.Empty<DesenvolvedoraEntity>();

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public DesenvolvedoraEntity? ObterUm(int Id)
        {
            try
            {
                var desenvolvedora = _context
                    .Desenvolvedora
                    .Include(x => x.Jogos)
                    .FirstOrDefault(x => x.Id == Id);

                if (desenvolvedora is null)
                    return null;

                return desenvolvedora;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
