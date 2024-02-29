
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Users.Events;
namespace CleanArchitecture.Domain.Users;

public class User : Entity
{
    //private set; is used to make the property immutable
    //Ningún valor puede ser asignado a la propiedad fuera del constructor
    //private set; no puede cambiar el valor de la propiedad fuera del constructor
    public Nombre? Name { get; private set; }
    public Apellido? Apellido { get; private set; }
    public Email? Email { get; private set; }


    private User(Guid id, string name, string lastname, string email) : base(id)
    {
        this.Name = new Nombre(name);
        this.Apellido = new Apellido(lastname);
        this.Email = new Email(email);
    }

    //Cualquier método que necesite crear una instancia de User, debe hacerlo a través de este método
    //static: porque no necesitamos una instancia de la clase para llamar a este método
    public static User Create(Guid id, string name, string lastname, string email)
    {
        var user = new User(id, name, lastname, email);

        
        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
        return user;
    }


}
