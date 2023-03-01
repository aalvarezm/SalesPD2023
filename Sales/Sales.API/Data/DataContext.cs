using Microsoft.EntityFrameworkCore;
using Sales.Shared.Entities;

namespace Sales.API.Data
{
    public class DataContext : DbContext //Hereda
    {
        //Constructor de la clase datacontext
        public DataContext(DbContextOptions<DataContext> options) : base(options) //se pasan los parámetros
        {
        }
        public DbSet<Country> Countries { get; set; } // se plularizara

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique(); //
        }
    }
}
