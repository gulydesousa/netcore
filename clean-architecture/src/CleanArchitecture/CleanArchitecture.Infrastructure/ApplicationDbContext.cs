using CleanArchitecture.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure;

//IUnitOfWork es una interfaz que se encarga de definir los métodos 
//que se deben implementar en la clase ApplicationDbContext
//Se encarga de enviar a la base de datos los cambios que estan en 
//la persistencia de Entity Framework a la base de datos
public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {   

    }
}
