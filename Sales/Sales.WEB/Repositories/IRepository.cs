namespace Sales.WEB.Repositories
{
    public interface IRepository
    {
        Task<HttpResponseWrapper<T>> Get<T>(string url);//Pido un Get de T, osea de lo que sea, Get de paises, Get de Ciudades, Get de productos, etc., la URL es como yo rutie el controlador

        Task<HttpResponseWrapper<object>> Post<T>(string url, T model);//Post que no devuelve nada

        Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T model);//De tipo Tresponse, osea un objeto de tipo respuesta, devuelve una lista
    }
    
}
