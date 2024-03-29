using CleanArchitecture.Api.Extensions;
using CleanArchitecture.Application;
using CleanArchitecture.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

//Método de extensión que agrega servicios de controladores
//a la colección de servicios.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Agregar los Dependency Injection de las referencias de la aplicación.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Aplicar la migración a la base de datos.
app.ApplyMigration();
app.SeedData();
app.UserCustomExceptionHandler();

//Mapear los controladores de la aplicación en la ruta base de la aplicación.
//y app.Run es para ejecutar la aplicación.
app.MapControllers();

app.Run();

