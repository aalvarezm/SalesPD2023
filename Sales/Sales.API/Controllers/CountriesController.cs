using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.Shared.Entities;

namespace Sales.API.Controllers
{
    //Defino a través del data annotations que esta clase es un controlador
    [ApiController]
    [Route("/api/countries")] //Rutear para llamar al controlador
    public class CountriesController: ControllerBase
    {
        private readonly DataContext _context;

        public CountriesController(DataContext context) //Le inyecto el datacontext
        {
            _context = context; //Refactorizo, para mi context es llamar toda la base de datos, se llama la instancia
            
        }


        //Obtener de forma asincroniza el listado de paises 
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _context.Countries.ToListAsync());
        }




        [HttpPost]
        public async Task<ActionResult> PostAsync(Country country) //Tengo que pasarle los parametros al metodo post, en este caso un país
        {
            _context.Add(country); //Le estoy diciendo al metodo que inserte un nuevo país
            await _context.SaveChangesAsync();
            return Ok(country); //Le digo que me devuela el país como quedo insertado
        }//Task es el void de los asincronos
            //Action result son todas las respuestas de HTTP
    }
}
