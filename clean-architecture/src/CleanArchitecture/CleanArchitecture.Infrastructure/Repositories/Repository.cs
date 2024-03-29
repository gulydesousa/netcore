using CleanArchitecture.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories;
public class Repository<T>
where T : Entity
{
    //ApplicationDbContext es la clase que se encarga de la conexión con la base de datos
    protected readonly ApplicationDbContext DbContext;


    protected Repository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    //Para obtener un registro de la base de datos por su id
    public async Task<T?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        return await DbContext.Set<T>()
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }


    //Para agregar un nuevo registro a la base de datos
    public void Add(T entity)
    {
        DbContext.Add(entity);
    }
}