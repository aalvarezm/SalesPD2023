using System.Text;
using System.Text.Json;

namespace Sales.WEB.Repositories
    //CLASE GENERICA
{
    public class Repository : IRepository
    {
        private readonly HttpClient _httpClient;

        private JsonSerializerOptions _jsonDefaultOptions => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,//Propiedad que no le importa si llega en Mayuscula o minuscula
        };

        public Repository(HttpClient httpClient)//Inyectamos el Htttpclient
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseWrapper<T>> Get<T>(string url)//El generico del generico
        {
            var responseHttp = await _httpClient.GetAsync(url);//Espera la respuesta del get de forma asincrona
            if (responseHttp.IsSuccessStatusCode)//Si la respuesta http es correcta
            {
                var response = await UnserializeAnswer<T>(responseHttp, _jsonDefaultOptions);// Leo la respuesta y a deseserializo
                return new HttpResponseWrapper<T>(response, false, responseHttp);
                //Devuelve la respuesta, false porque no hay error y mando la respuesta que retorno el servidor
            }

            return new HttpResponseWrapper<T>(default, true, responseHttp);
        }

        //POST que no devuelve nada
        public async Task<HttpResponseWrapper<object>> Post<T>(string url, T model)
        {
            var mesageJSON = JsonSerializer.Serialize(model);
            var messageContet = new StringContent(mesageJSON, Encoding.UTF8, "application/json");//Codifico el contenido del mensaje
            var responseHttp = await _httpClient.PostAsync(url, messageContet);
            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        public async Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T model)
        {
            var messageJSON = JsonSerializer.Serialize(model);
            var messageContet = new StringContent(messageJSON, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PostAsync(url, messageContet);
            if (responseHttp.IsSuccessStatusCode)//Si la respuesta es correcta
            {
                var response = await UnserializeAnswer<TResponse>(responseHttp, _jsonDefaultOptions);
                return new HttpResponseWrapper<TResponse>(response, false, responseHttp);
            }
            return new HttpResponseWrapper<TResponse>(default, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        private async Task<T> UnserializeAnswer<T>(HttpResponseMessage httpResponse, JsonSerializerOptions jsonSerializerOptions)
        {
            var respuestaString = await httpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(respuestaString, jsonSerializerOptions)!;
        }



        //IMPLEMENTAMOS
        public async Task<HttpResponseWrapper<object>> Delete(string url)//Como el metodo no devuelve nada
        {
            var ResponseHTTP = await _httpClient.DeleteAsync(url);// Eliminamos y esto lo guardamos en la variable
            return new HttpResponseWrapper<object>(null, !ResponseHTTP.IsSuccessStatusCode,ResponseHTTP);//Devolvemos una respuesta de lo que el metodo devolvió o de lo que el servicio devolvió
        }

        public async Task<HttpResponseWrapper<object>> Put<T>(string url, T model)// Como el metodo no devuelve nada
        {
            var messageJSON = JsonSerializer.Serialize(model);//Serializo el modelo, es decir, coger un objeto y volverlo string
            var messageContent = new StringContent(messageJSON, Encoding.UTF8, "application/json");//codifico el el modelo de arriba, lo estamos encodificando a UTF-8
            var responseHTTP = await _httpClient.PutAsync(url, messageContent);//Despues lo mandamos al metodo PostAsync
            return new HttpResponseWrapper<object>(null, !responseHTTP.IsSuccessStatusCode, responseHTTP);// Devolvemos la respuesta haga o no haga la actualización
        }


        public async Task<HttpResponseWrapper<TResponse>> Put<T, TResponse>(string url, T model)
        {
            var messageJSON = JsonSerializer.Serialize(model);//Serializo el modelo, es decir, coger un objeto y volverlo string
            var messageContent = new StringContent(messageJSON, Encoding.UTF8, "application/json");//codifico el el modelo de arriba, lo estamos encodificando a UTF-8
            var responseHTTP = await _httpClient.PutAsync(url, messageContent);//Despues lo mandamos al metodo PostAsync
           // Devolvemos la respuesta haga o no haga la actualización


            if (responseHTTP.IsSuccessStatusCode)//Si la respuesta es correcta
            {
                var response = await UnserializeAnswer<TResponse>(responseHTTP, _jsonDefaultOptions);//Deserializamos la respuesta
                return new HttpResponseWrapper<TResponse>(response, false, responseHTTP);//Despues enviamos la respuesta
            }
            return new HttpResponseWrapper<TResponse>(default, !responseHTTP.IsSuccessStatusCode, responseHTTP);
        }

  
    }
}
