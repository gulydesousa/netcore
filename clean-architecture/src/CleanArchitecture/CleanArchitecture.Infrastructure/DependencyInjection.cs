using CleanArchitecture.Application.Abstractions.Clock;
using CleanArchitecture.Application.Abstractions.Data;
using CleanArchitecture.Application.Abstractions.Email;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehiculos;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Email;
using CleanArchitecture.Infrastructure.Repositories;
using Dapper;
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

        //Conexion que se utiliza para conectarse a la base de datos
        //Para dapper
        services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));

        //IUnitOfWork es una interfaz que se encarga de hacer el commit de los cambios
        //que se hacen en la base de datos
        //Esto se hace para que se puedan hacer transacciones en la base de datos
        //y se puedan hacer rollback de los cambios
        services.AddScoped<IUnitOfWork>(provider => provider.GetService<ApplicationDbContext>()!);

        //Agregamos los servicios de la capa de infraestructura
        //Estos servicios son los que se encargan de la conexión con la base de datos
        //y de la implementación de las interfaces de la capa de aplicación
        //AddScoped es un método que se encarga de agregar los servicios a la inyección de dependencias
        //Estos servicios se van a mantener vivos mientras la petición este viva
        //Esto quiere decir que si se hace una petición a la base de datos
        //y se hace otra petición a la base de datos en el mismo request
        //Se va a mantener la misma conexión a la base de datos
        //Esto es útil para no tener que abrir y cerrar la conexión a la base de datos
        //cada vez que se haga una petición
        services.AddScoped<IAlquilerRepository, AlquilerRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IVehiculoRepository, VehiculoRepository>();



        //Se agrega el tipo de manejador de fecha
        //Esto se hace para que se pueda hacer el mapeo de las fechas
        //de la base de datos a las fechas de C#
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

        return services;
    }
}
