using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.Shared.Entities;

namespace Sales.API.Controllers
{
    //A cada controlador le agrego estas dos data annotations porque esto es lo que hace que sea un controlador
    [ApiController]
    [Route("/api/states")]
    public class StatesController : ControllerBase //Hereda para convertirlo en un controlador
    {
        private readonly DataContext _context; //Si tiene un "_" significa que es un atributo de clase

        public StatesController(DataContext context) {
            _context = context; //Esto signfica al atributo privado de la clase, llevele lo que le llegó por parametro del context
        }

        //Obtener de forma asincroniza el listado de paises 
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _context.States
                .Include(x => x.Cities) //Esto es equivalente "select * from paises INNER JOIN states 
                .ToListAsync());
        }


         
        //Obtener de forma asincroniza el ID de un SOLO pais
        // METODO PARA LISTAR UN SOLO PAIS POR SU ID
        [HttpGet("{id:int}")]//Le pasamos un parámetro al metodo
        public async Task<IActionResult> GetAsync(int id)
        {
            var State = await _context.States
                .Include(x => x.Cities) 
                .FirstOrDefaultAsync(x => x.Id == id);// creo la variable para preguntar si el país existe
            //Adicional creo una función lambda para preguntar si el id es igual al id que me pasaron como parámetro
            if (State == null)// Pregunto si el país existe
            {
                return NotFound();
            }
            return Ok(State); //Retorno el país que encontró
        }



        [HttpPut] //METODO PARA ACTUALIZAR 
        //Task es el void de los asincronos
        //Action result son todas las respuestas de HTTP
        public async Task<ActionResult> PutAsync(State state) //Tengo que pasarle los parametros al metodo put, en este caso un país
        {
            try
            {
                _context.Update(state);//Le estoy diciendo al metodo que actualice un nuevo país
                await _context.SaveChangesAsync();//Le digo que me devuela el país como quedo actualizado
                return Ok(state);
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
        public async Task<ActionResult> PostAsync(State state) //Tengo que pasarle los parametros al metodo post, en este caso un país
        {
            try

            { 
                _context.Add(state);//Le estoy diciendo al metodo que inserte un nuevo país
                await _context.SaveChangesAsync();
                return Ok(state);//Le digo que me devuela el país como quedo insertado
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


        //METODO PARA ELIMINAR UN PAÍS
        [HttpDelete("{id:int}")]//Le pasamos un parámetro al metodo
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var state = await _context.States.FirstOrDefaultAsync(x => x.Id == id);// creo la variable para preguntar si el país existe
            //Adicional creo una función lambda para preguntar si el id es igual al id que me pasaron como parámetro
            if (state == null)// Pregunto si el país existe
            {
                return NotFound();
            }
            _context.Remove(state); //Le estoy diciendo al metodo que elimine el país
            await _context.SaveChangesAsync();

            return NoContent(); //Respuesta para borrar un país
        }


    }
}
