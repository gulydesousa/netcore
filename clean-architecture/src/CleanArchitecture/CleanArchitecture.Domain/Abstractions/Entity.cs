namespace CleanArchitecture.Domain.Abstractions
{
    //Clase abstracta: No puede ser instanciada, solo puede ser heredada
    //Solo sus hijos pueden ser instanciados
    public abstract class Entity
    {
        //protected: solo puede ser accedido por las clases que heredan de esta clase
        protected Entity() { }

        //Lista de eventos de dominio
        //Solo puede ser accedida por la clase que hereda de esta clase
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

        //Identificador único de cada entidad de nuestro dominio
        //El identificador es de solo lectura, es decir, no se puede modificar
        //init: una vez que se crea una entidad, su identificador no cambia de por vida
        public Guid Id { get; init; }

        //constructor de la clase
        //protected: solo puede ser accedido por las clases que heredan de esta clase
        protected Entity(Guid id)
        {
            //Si el identificador es igual a Guid.Empty
            if (id == Guid.Empty)
            {
                //Generamos un nuevo identificador
                Id = Guid.NewGuid();
            }
            else
            {
                //Si no, el identificador es igual al identificador que se le pasa por parámetro
                Id = id;
            }
        }

        //Método que agrega un evento de dominio a la lista de eventos de dominio
        //protected: solo puede ser accedido por las clases que heredan de esta clase
        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            //Agregamos el evento de dominio a la lista de eventos de dominio
            _domainEvents.Add(domainEvent);
        }

        //Método que limpia la lista de eventos de dominio
        //protected: solo puede ser accedido por las clases que heredan de esta clase
        public void ClearDomainEvents()
        {
            //Limpiamos la lista de eventos de dominio
            _domainEvents.Clear();
        }

        //Método que obtiene la lista de eventos de dominio
        //protected: solo puede ser accedido por las clases que heredan de esta clase
        public IReadOnlyList<IDomainEvent> GetDomainEvents()
        {
            //Retornamos la lista de eventos de dominio
            return _domainEvents.ToList();
        }

        //Método que lanza un evento de dominio
        //protected: solo puede ser accedido por las clases que heredan de esta clase
        protected void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            //Agregamos el evento de dominio a la lista de eventos de dominio
            AddDomainEvent(domainEvent);
        }
    }
}