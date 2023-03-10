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
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }

        //Crearle un indice uno a cada país, con el metodo overrride
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique();

            //Creo un indice compuesto, esto permite que por ejemplo exista una sola Antioquia en toda Colombia, pero Antioquia pueden existir en otros paises, es decir, la relacion directa relacionad a aun pais, solo una vez, pero que luego el estado pueda relacionarse con cualquier otro pais
            //Le creo el indice al estado
            modelBuilder.Entity<State>().HasIndex("CountryId","Name").IsUnique(); // aunque no haya creado la propiedad CountryId el internamente entiende que ese es el Id de la tabla Country sobre la cuál se va a relacionar con Estado

            //Le creo el indice a la ciudad
            modelBuilder.Entity<City>().HasIndex("StateId","Name").IsUnique();
        }
    }
}
