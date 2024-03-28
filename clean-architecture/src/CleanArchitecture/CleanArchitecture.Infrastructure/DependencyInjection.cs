using CleanArchitecture.Application.Abstractions.Clock;
using CleanArchitecture.Application.Abstractions.Email;
using CleanArchitecture.Infrastructure.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IEmailService, EmailService>();

        //Conexion que se utiliza para conectarse a la base de datos
        //Para entity framework y dapper
        var connectionString = configuration.GetConnectionString("Database")
        ?? throw new ArgumentNullException(nameof(configuration));

        //Se agrega el contexto de la base de datos a la inyección de dependencias
        //Esto nos sirve tambien para poder hacer las migraciones
        //y poder hacer las consultas a la base de datos
        //Queremos hacer un mapeo de los nombres de las tablas a los nombres de las clases
        //UseSnakeCaseNamingConvention() es un método que se encarga de hacer el mapeo
        //de los nombres de las tablas a los nombres de las clases
        //Esto se hace para que sea más fácil de leer y de entender
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString).UseSnakeCaseNamingConvention());


        return services;
    }
}
