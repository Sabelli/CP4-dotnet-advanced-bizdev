using Jogos.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jogos.API.Infrastructure.Data
{
    public class ApplicationContext:DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }

        public DbSet<CategoriaEntity> Cliente { get; set; }
        public DbSet<DesenvolvedoraEntity> Produto { get; set; }
        public DbSet<JogoEntity> Curso { get; set; }
    }
}
