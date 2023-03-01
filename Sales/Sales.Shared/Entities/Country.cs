using System.ComponentModel.DataAnnotations;

namespace Sales.Shared.Entities
{
    public class Country
    {
        //Todas entidades tienen un Id
        public int Id { get; set; }

        //Manejo de DataAnnotations permite cambiarle el comportamiento a las entidades
        // El 0 y el 1 se reemplazan por el orden de los dataannotations

        [Display(Name = "País")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!; // Le indico que no será nulo

    }
}
