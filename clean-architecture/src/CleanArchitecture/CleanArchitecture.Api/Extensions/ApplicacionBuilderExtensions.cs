using CleanArchitecture.Api.Middleware;
using CleanArchitecture.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Api.Extensions;
public static class ApplicacionBuilderExtensions
{
    //Extension method to apply migration to the database
    //IApplicationBuilder is the interface that defines the 
    //contract for the application's request processing pipeline
    //The ApplyMigration method is an extension method that extends the IApplicationBuilder interface
    //The ApplyMigration method is used to apply the migration to the database
    public static async void ApplyMigration(this IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            var service = serviceScope.ServiceProvider;
            var loggerFactory = service.GetRequiredService<ILoggerFactory>();
            //Si todo es correcto creará las tablas en la base de datos
            try
            {
                var context = service.GetRequiredService<ApplicationDbContext>();
                await context.Database.MigrateAsync();
            }
            //Si hay un error en la migración, se captura la excepción y se muestra en la consola
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger("app");
                logger.LogError(ex, "Error al aplicar migraciones a la base de datos");
            }

        }
    }

    //Extension method to handle exceptions in the application
    public static void UserCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
