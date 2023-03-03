using System.Net;

namespace Sales.WEB.Repositories
{

    //Clase que coge las respuestas del servidor y las coloca como genericas para que me sirva para todo el proyecto
    public class HttpResponseWrapper<T>//La T se reemplaza por la definición que este usando
    {
        //Primero va, atributos privados, luego constructores, propiedades, metodos publicos y metodos privados
        //T? Significa que puede recibir el parametro nulo
        //El 2do parametro es capturar el error
        //El 3er parametro es obtener el mensaje 
        public HttpResponseWrapper(T? response, bool error, HttpResponseMessage httpResponseMessage)
        {
            Error = error;
            Response = response;
            HttpResponseMessage = httpResponseMessage;
        }

        public bool Error { get; set; }

        public T? Response { get; set; }

        public HttpResponseMessage HttpResponseMessage { get; set; }

        public async Task<string?> GetErrorMessage()
        {
            if (!Error)//Si el metodo es diferente de error, retorne null, no paso nada
            {
                return null;
            } 
            //Pero si si existe algun error, deseo guardarlo en una variable

            var statusCode = HttpResponseMessage.StatusCode;
            if (statusCode == HttpStatusCode.NotFound)
            {
                return "Recurso no encontrado";
            }
            else if (statusCode == HttpStatusCode.BadRequest)
            {
                return await HttpResponseMessage.Content.ReadAsStringAsync();
            }
            else if (statusCode == HttpStatusCode.Unauthorized)
            {
                return "Tienes que logearte para hacer esta operación";
            }
            else if (statusCode == HttpStatusCode.Forbidden)
            {
                return "No tienes permisos para hacer esta operación";
            }

            return "Ha ocurrido un error inesperado";
        }
    }

}
