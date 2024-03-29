using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure;
//IUnitOfWork es una interfaz que se encarga de definir los métodos 
//que se deben implementar en la clase ApplicationDbContext
//Se encarga de enviar a la base de datos los cambios que estan en 
//la persistencia de Entity Framework a la base de datos
public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
    //IPublisher es una interfaz que se encarga de definir los métodos
    //que se deben implementar en la clase ApplicationDbContext
    //Se encarga de enviar los eventos de dominio a la base de datos 
    private readonly IPublisher _publisher;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IPublisher publisher) : base(options)
    {
        _publisher = publisher;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Se encarga de buscar todas las clases que implementan IEntityTypeConfiguration
        //El assembly contiene el dbcontext y todas las clases que implementan IEntityTypeConfiguration
        //Aplica todas las configuraciones de las clases que implementan IEntityTypeConfiguration
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    private async Task PublishDomainEventsAsync(CancellationToken cancellationToken)
    {
        //Se encarga de enviar a la base de datos los cambios que estan en
        //la persistencia de Entity Framework a la base de datos
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(e => e.Entity)
            .SelectMany(x =>
            {
                var domainEvents = x.GetDomainEvents();
                x.ClearDomainEvents();
                return domainEvents;
            }).ToList();

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }

    }

    //Se encarga de enviar a la base de datos los cambios que estan en
    //la persistencia de Entity Framework a la base de datos
    //Los DomainEvents son los que se encargan de enviar los eventos de dominio a la base de datos
    //Un ejemplo de un DomainEvent es el evento de creación de un usuario en la base de datos
    //El evento de creación de un usuario se encarga de enviar a la base de datos la creación de un usuario
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            await PublishDomainEventsAsync(cancellationToken);
            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            //Ha ocurrido una violación en las reglas de BBDD
            //Se ha disparado la exception por concurrencia
            //Podremos usar esta nueva excepción: ConcurrencyException para manejarla en la capa de aplicación
            //El propósito de esta clase es representar una excepción de concurrencia 
            //en la aplicación Clean Architecture. 
            //Las excepciones de concurrencia suelen ocurrir cuando múltiples hilos o procesos 
            //intentan acceder o modificar los mismos datos al mismo tiempo, 
            //lo que puede llevar a resultados inesperados o inconsistentes.
            throw new ConcurrencyException("Se ha disparado la exception por concurrencia", ex);
        }
    }
}
