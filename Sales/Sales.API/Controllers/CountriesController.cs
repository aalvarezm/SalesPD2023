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


        //Obtener de forma asincroniza el ID de un SOLO pais
        // METODO PARA LISTAR UN SOLO PAIS POR SU ID
        [HttpGet("{id:int}")]//Le pasamos un parámetro al metodo
        public async Task<IActionResult> GetAsync(int id)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(x => x.Id == id);// creo la variable para preguntar si el país existe
            //Adicional creo una función lambda para preguntar si el id es igual al id que me pasaron como parámetro
            if(country == null)// Pregunto si el país existe
            {
                return NotFound();
            }
            return Ok(country); //Retorno el país que encontró
        }



        [HttpPut] //METODO PARA ACTUALIZAR 
        //Task es el void de los asincronos
        //Action result son todas las respuestas de HTTP
        public async Task<ActionResult> PutAsync(Country country) //Tengo que pasarle los parametros al metodo put, en este caso un país
        {
            try
            {
                _context.Update(country);//Le estoy diciendo al metodo que actualice un nuevo país
                await _context.SaveChangesAsync();//Le digo que me devuela el país como quedo actualizado
                return Ok(country);
            }
            catch (DbUpdateException DbUpdateException)//Hubo un error en la actualizacion de datos
            {
                //Capturo el error de la db donde le digo que si el mensaje de error contiene la palabra duplicate
                if (DbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    //Le retorno un badrequest pero personalizado
                    return BadRequest("Ya existe un pais con el mismo nombre");
                }
                return BadRequest(DbUpdateException.Message);

            }
            catch (Exception ex)//CATCH para capturar si es otro cualquier mensaje de error, capture pero en el objeto ex
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost] //METODO PARA CREAR 
        //Task es el void de los asincronos
        //Action result son todas las respuestas de HTTP
        public async Task<ActionResult> PostAsync(Country country) //Tengo que pasarle los parametros al metodo post, en este caso un país
        {
            try

            {
                _context.Add(country);//Le estoy diciendo al metodo que inserte un nuevo país
                await _context.SaveChangesAsync();
                return Ok(country);//Le digo que me devuela el país como quedo insertado
            }

            catch (DbUpdateException DbUpdateException)//Hubo un error en la actualizacion de datos
            {
                //Capturo el error de la db donde le digo que si el mensaje de error contiene la palabra duplicate
                if (DbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    //Le retorno un badrequest pero personalizado
                    return BadRequest("Ya existe un pais con el mismo nombre");
                }
                return BadRequest(DbUpdateException.Message);
                
            }
            catch(Exception ex)//CATCH para capturar si es otro cualquier mensaje de error, capture pero en el objeto ex
            {return BadRequest(ex.Message);
            }
        }


        //METODO PARA ELIMINAR UN PAÍS
        [HttpDelete("{id:int}")]//Le pasamos un parámetro al metodo
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(x => x.Id == id);// creo la variable para preguntar si el país existe
            //Adicional creo una función lambda para preguntar si el id es igual al id que me pasaron como parámetro
            if (country == null)// Pregunto si el país existe
            {
                return NotFound();
            }
            _context.Remove(country); //Le estoy diciendo al metodo que elimine el país
            await _context.SaveChangesAsync();

            return NoContent(); //Respuesta para borrar un país
        }




    }
}
