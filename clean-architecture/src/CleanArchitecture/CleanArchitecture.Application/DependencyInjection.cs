using CleanArchitecture.Domain.Alquileres;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using CleanArchitecture.Application.Abstractions.Behaviours;
using FluentValidation;

namespace CleanArchitecture.Application // Fix the namespace
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
/*
            services.AddMediatR(
                configuration =>
                {
                    configuration.RegisterServiceFromAssembly(typeof(DependencyInjection).Assembly);
                    configuration.AddOpenBehavior(typeof(LoggingBehaviour<,>));
                }
            );
*/
            services.AddMediatR(typeof(DependencyInjection).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            
            services.AddTransient<PrecioService>();

            //Escanea todos los ensamblados que contienen clases que 
            //heredan de FluentValidation.AbstractValidator
            //y las registra en el contenedor de inyección de dependencias
            services.AddValidatorsFromAssemblies(new[] { typeof(DependencyInjection).Assembly });
            
            return services;
        }
    }
}