using Microsoft.EntityFrameworkCore;

namespace Jogos.API.Infrastructure.Data
{
    public class ApplicationContext:DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }
    }
}
