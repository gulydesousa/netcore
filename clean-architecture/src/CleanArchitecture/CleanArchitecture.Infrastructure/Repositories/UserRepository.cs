using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Infrastructure.Repositories;

//IUserRepository es una interfaz que se encarga de definir los métodos
//que se deben implementar en la clase UserRepository
//internal sealed class UserRepository : Repository<User>, IUserRepository
//Significa que UserRepository hereda de Repository<User> e implementa IUserRepository
//UserRepository es una clase que se encarga de realizar las operaciones de base de datos
//con la entidad User
internal sealed class UserRepository : Repository<User>, IUserRepository
{

    //Aqui va la implementación personalizada de los métodos de IUserRepository
    //Si no hay implementación personalizada, no es necesario implementar la interfaz
    //ya que UserRepository hereda de Repository<User> que ya implementa IUserRepository

    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
