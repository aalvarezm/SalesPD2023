using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Instruccion que sirve para ignorar las referencias ciclicas
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer("name=LocalConnection")); //inyectarle el string de conexión, sin importar la clase
builder.Services.AddTransient<Seeddb>();//Uso el transient porque lo necesito inyectar solo una vez y ya

var app = builder.Build();

Seeddb(app);

void Seeddb(WebApplication app)//Inyeccion a mano
{
    IServiceScopeFactory? scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (IServiceScope? scope = scopedFactory!.CreateScope())
    {
        Seeddb? service = scope.ServiceProvider.GetService<Seeddb>();
        service!.SeedAsync().Wait();//Uso el wait porque esta clase no es asincrona
    }

}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//Habilito el cors para poder hacer las peticiones
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());


app.Run();
