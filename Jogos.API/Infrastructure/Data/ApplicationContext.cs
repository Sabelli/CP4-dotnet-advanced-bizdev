using Jogos.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jogos.API.Infrastructure.Data
{
    public class ApplicationContext:DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }

        public DbSet<CategoriaEntity> Categoria { get; set; }
        public DbSet<DesenvolvedoraEntity> Desenvolvedora { get; set; }
        public DbSet<JogoEntity> Jogo { get; set; }
    }
}
