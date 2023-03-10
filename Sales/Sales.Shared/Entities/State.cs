using System.ComponentModel.DataAnnotations;

namespace Sales.Shared.Entities
{
    public class State
    {
        //Todas entidades tienen un Id
        public int Id { get; set; }

        //Manejo de DataAnnotations permite cambiarle el comportamiento a las entidades
        // El 0 y el 1 se reemplazan por el orden de los dataannotations

        [Display(Name = "Estado/Departamento")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!; // Le indico que no será nulo

        public int CountryId { get; set; }//Creo una variable de tipo entera que me almacenará el Id del pais, esto me serivrá para que cuando el cliente regrese de editar el pais, lo regrese al pais que estaba editando, osea el id del pais

        //PARA LAS RELACIONES


        //Definimos la relación, en este caso en doble sentido, en estados se crea la propiedad country para la relacionarse los estados a un pais y acá en county creo la propiedad de tipo IColecction que me relacione la coleccion de estados

        //Un estado (entidad en la que estoy parado, tiene una coleccion de muchas ciudades)
        public ICollection<City>? Cities { get; set; }

        //Creo una propiedad para saber la cantidad de estados que tengo

        public int CitiesNumbers => Cities == null ? 0 : Cities.Count; //Operador ternario

        //un estado pertenece a un país y un país tiene muchos estados --> relacion 1 a muchos
        public Country? Country { get; set; }

        
    }
}
