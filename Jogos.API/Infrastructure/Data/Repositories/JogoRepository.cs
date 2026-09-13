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

        public JogoEntity? Adicionar(JogoEntity entity)
        {
            try
            {
                _context.Jogo.Add(entity);
                _context.SaveChanges();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public JogoEntity? Deletar(int Id)
        {
            try
            {
                var jogo = _context.Jogo.FirstOrDefault(x => x.Id == Id);

                if (jogo is null)
                    return null;

                _context.Jogo.Remove(jogo);
                _context.SaveChanges();

                return jogo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public JogoEntity? Editar(int Id, JogoEntity entity)
        {
            try
            {
                var jogo = _context
                    .Jogo
                    .Include(x => x.Categorias)
                    .FirstOrDefault(x => x.Id == Id);

                if (jogo is null)
                    return null;

                jogo.Nome = entity.Nome;
                jogo.Preco = entity.Preco;
                jogo.Plataforma = entity.Plataforma;
                jogo.DataLancamento = entity.DataLancamento;
                jogo.DesenvolvedoraId = entity.DesenvolvedoraId;

                _context.Jogo.Update(jogo);
                _context.SaveChanges();

                return jogo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<PageResultModel<IEnumerable<JogoEntity>>> ObterTodosAsync(int Deslocamento = 0, int RegistroRetornado = 30)
        {
            try
            {
                if (Deslocamento < 0) Deslocamento = 0;
                if (RegistroRetornado <= 0) RegistroRetornado = 30;

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

        public JogoEntity? ObterUm(int Id)
        {
            try
            {
                var jogo = _context
                    .Jogo
                    .Include(x => x.Desenvolvedora)
                    .Include(x => x.Categorias)
                    .FirstOrDefault(x => x.Id == Id);

                if (jogo is null)
                    return null;

                return jogo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
