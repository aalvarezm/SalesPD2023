using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Sales.WEB;
using Sales.WEB.Repositories;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7219/") });
builder.Services.AddScoped<IRepository, Repository>();//Realizar la inyeccion por la interfaz, no por la clase que implementa

//Addscoped: me crea una instancia del objeto cada que se haga una inyeccion
//Singleton: Casos especificos (preguntar al docente)
//Transient: Inyectar solo una vez

await builder.Build().RunAsync();
