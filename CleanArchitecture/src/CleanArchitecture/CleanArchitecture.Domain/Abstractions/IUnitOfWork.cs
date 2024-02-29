namespace CleanArchitecture.Domain.Abstractions;
public interface IUnitOfWork
{
    //Método que guarda los cambios en la base de datos
    //CancelationToken: permite cancelar una operación asincrónica
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}