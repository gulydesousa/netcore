namespace CleanArchitecture.Domain.Users;

public interface IUserRepository
{
    //Método que agrega un usuario a la base de datos
    //CancelationToken: permite cancelar una operación asincrónica
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    void Add(User user);
  
}