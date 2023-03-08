using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Sales.WEB;
using Sales.WEB.Repositories;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7219/") });
builder.Services.AddScoped<IRepository, Repository>();//Realizar la inyeccion por la interfaz, no por la clase que implementa
//Inyecto la libreria del sweet alert 2 que me permitirá ventanas modales muy llamativas para el usuario
builder.Services.AddSweetAlert2();

//Addscoped: me crea una instancia del objeto cada que se haga una inyeccion
//Singleton: Casos especificos (preguntar al docente)
//Transient: Inyectar solo una vez

await builder.Build().RunAsync();

