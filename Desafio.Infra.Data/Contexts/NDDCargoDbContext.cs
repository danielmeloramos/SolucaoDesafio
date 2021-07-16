using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Desafio.Infra.Data.Contexts
{
    /// <summary>
    /// Contexto de banco de dados do NDDCargo
    /// </summary>
    public class NDDCargoDbContext : DbContext
    {
        public NDDCargoDbContext(DbContextOptions<NDDCargoDbContext> options) : base(options) => TryApplyMigration(options);

        /// <summary>
        /// Método que é executado quando o modelo de banco de dados está sendo criado pelo EF.
        /// Útil para realizar configurações
        /// </summary>
        /// <param name="modelBuilder">É o construtor de modelos do EF</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(true);

            base.OnConfiguring(optionsBuilder);
        }

        private void TryApplyMigration(DbContextOptions<NDDCargoDbContext> options)
        {
            var inMemoryConfiguration = options.Extensions.FirstOrDefault(x => x.ToString().Contains("InMemoryOptionsExtension"));

            if (inMemoryConfiguration == null && Database.GetPendingMigrations().Count() > 0)
                Database.Migrate();
        }
    }
}
