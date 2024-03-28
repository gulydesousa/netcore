using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Users.Events;
namespace CleanArchitecture.Domain.Users;

//sealed: no se puede heredar de esta clase
public sealed class User : Entity
{
    //private set; is used to make the property immutable
    //Ningún valor puede ser asignado a la propiedad fuera del constructor
    //private set; no puede cambiar el valor de la propiedad fuera del constructor
    public Nombre? Name { get; private set; }
    public Apellido? Apellido { get; private set; }
    public Email? Email { get; private set; }

    private User(Guid id, Nombre name, Apellido lastname, Email email) : base(id)
    {
        this.Name = name;
        this.Apellido = lastname;
        this.Email = email;
    }

    //Cualquier método que necesite crear una instancia de User, debe hacerlo a través de este método
    //static: porque no necesitamos una instancia de la clase para llamar a este método
    public static User Create(Nombre name, Apellido lastname, Email email)
    {
        var user = new User(Guid.NewGuid(), name, lastname, email);
        //RaiseDomainEvent: método de la clase base Entity
        //UserCreatedDomainEvent: evento de dominio que se levanta cuando se crea un usuario              
        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
        return user;
    }
}
