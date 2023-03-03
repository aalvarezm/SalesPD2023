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
    }
}
