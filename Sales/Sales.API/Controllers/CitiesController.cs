using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.Shared.Entities;

namespace Sales.API.Controllers
{
    //Defino a través del data annotations que esta clase es un controlador
    [ApiController]
    [Route("/api/cities")] //Rutear para llamar al controlador
    public class CitiesController : ControllerBase
    {
        private readonly DataContext _context;

        public CitiesController(DataContext context)//Le inyectamos el datacontex
        {
            _context = context;
        }

        //Obtener de forma asincroniza el listado de paises 
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _context.Cities.ToListAsync());
        }



        //Obtener de forma asincroniza el ID de un SOLO pais
        // METODO PARA LISTAR UN SOLO PAIS POR SU ID
        [HttpGet("{id:int}")]//Le pasamos un parámetro al metodo
        public async Task<IActionResult> GetAsync(int id)
        {
            var City = await _context.Cities.FirstOrDefaultAsync(x => x.Id == id);// creo la variable para preguntar si el país existe
            //Adicional creo una función lambda para preguntar si el id es igual al id que me pasaron como parámetro
            if (City == null)// Pregunto si el país existe
            {
                return NotFound();
            }
            return Ok(City); //Retorno el país que encontró
        }



        [HttpPut] //METODO PARA ACTUALIZAR 
        //Task es el void de los asincronos
        //Action result son todas las respuestas de HTTP
        public async Task<ActionResult> PutAsync(City City) //Tengo que pasarle los parametros al metodo put, en este caso un país
        {
            try
            {
                _context.Update(City);//Le estoy diciendo al metodo que actualice un nuevo país
                await _context.SaveChangesAsync();//Le digo que me devuela el país como quedo actualizado
                return Ok(City);
            }
            catch (DbUpdateException DbUpdateException)//Hubo un error en la actualizacion de datos
            {
                //Capturo el error de la db donde le digo que si el mensaje de error contiene la palabra duplicate
                if (DbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    //Le retorno un badrequest pero personalizado
                    return BadRequest("Ya existe un Estado con el mismo nombre");
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
        public async Task<ActionResult> PostAsync(City city) //Tengo que pasarle los parametros al metodo post, en este caso un país
        {
            try

            {
                _context.Add(city);//Le estoy diciendo al metodo que inserte un nuevo país
                await _context.SaveChangesAsync();
                return Ok(city);//Le digo que me devuela el país como quedo insertado
            }

            catch (DbUpdateException DbUpdateException)//Hubo un error en la actualizacion de datos
            {
                //Capturo el error de la db donde le digo que si el mensaje de error contiene la palabra duplicate
                if (DbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    //Le retorno un badrequest pero personalizado
                    return BadRequest("Ya existe una ciudad con el mismo nombre");
                }
                return BadRequest(DbUpdateException.Message);

            }
            catch (Exception ex)//CATCH para capturar si es otro cualquier mensaje de error, capture pero en el objeto ex
            {
                return BadRequest(ex.Message);
            }
        }


        //METODO PARA ELIMINAR UN PAÍS
        [HttpDelete("{id:int}")]//Le pasamos un parámetro al metodo
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var City = await _context.Cities.FirstOrDefaultAsync(x => x.Id == id);// creo la variable para preguntar si el país existe
            //Adicional creo una función lambda para preguntar si el id es igual al id que me pasaron como parámetro
            if (City == null)// Pregunto si el país existe
            {
                return NotFound();
            }
            _context.Remove(City); //Le estoy diciendo al metodo que elimine el estado
            await _context.SaveChangesAsync();

            return NoContent(); //Respuesta para borrar una ciudad
        }
    }
}
