using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sales.Shared.Entities
{
    public class City
    {
        //Todas entidades tienen un Id
        public int Id { get; set; }

        //Manejo de DataAnnotations permite cambiarle el comportamiento a las entidades
        // El 0 y el 1 se reemplazan por el orden de los dataannotations

        [Display(Name = "Ciudad")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!; // Le indico que no será nulo

        //PARA LAS RELACIONES
        //una ciudad pertenece a un estado y un estado puede tener muchas ciudad -- > relacion 1 a muchos
        public State? State { get; set; }


    }
}
