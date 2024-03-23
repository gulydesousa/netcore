using CleanArchitecture.Domain.Alquileres;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace CleanArchitecture.Application // Fix the namespace
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(typeof(DependencyInjection).Assembly); 

            services.AddTransient<PrecioService>();

            return services;
        }
    }
}