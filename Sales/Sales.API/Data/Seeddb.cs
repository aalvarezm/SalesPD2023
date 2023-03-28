using Sales.Shared.Entities;

namespace Sales.API.Data
{
    //Un seeder en BD es un alimentador de datos, es decir, si borro la bd, el seeder me permite "poblar" la base de datos con registros quemados o con registros que yo le especifique, esto es bueno cuando necesito tablas como tipos de documentos o paises, que son tablas que en su estructura no variaran en el tiempo
    public class Seeddb
    {
        private readonly DataContext _context;

        public Seeddb(DataContext context)
        {
            _context = context;
        }

        //Metodo que alimenta la base de datos
        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();//Es update-database por código, crea la base de datos y corre las migraciones
            //Creo un metodo asincrono que me pregunte si la tabla paises ya tiene registros
            await CheckCountriesAsync();
        }

        //Metodo que valida 
        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())//El metodo Any devuelve true si al menos hay algun registro, entonces lo niego para decir "Si no hay registros..."
            {
                //Adicionamos paises hardcodeados o quemados

                //Agrego al seeder también estados y ciudades, no lo pienso como tablas de una BD, si no como objetos
                //Es como si paises, estados y ciudades fueran un solo objeto
                _context.Countries.Add(new Country
                {
                    Name = "Colombia",
                    States = new List<State> {
                        new State {
                            Name="Antioquia",
                            Cities = new List<City>
                            {
                                new City{ Name = "Medellín"},
                                new City{ Name = "Sabaneta"},
                                new City{ Name = "Envigado"},

                            }
                        },
                        new State {
                            Name="Santander",
                            Cities = new List<City>
                            {
                                new City{ Name = "Cimitarra"},
                                new City{ Name = "Girón"},
                                new City{ Name = "Bucaramanga"},
                            }
                        },
                        new State {
                            Name="Boyacá"
                        }
                    }
                });
                _context.Countries.Add(new Country { Name = "Brasil" });
                _context.Countries.Add(new Country
                {
                    Name = "Argentina",
                    States = new List<State>
                    {
                        new State
                        {
                            Name="Mar del plata",
                            Cities = new List<City>
                            {
                                new City{Name="Buenos Aires"},
                                new City{Name="San Lorenzo"},
                            }
                        }
                    }
                });
                _context.Countries.Add(new Country { Name = "Perú" });
                await _context.SaveChangesAsync();
            }
        }
    }
}
