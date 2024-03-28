using CleanArchitecture.Application.Abstractions.Clock;

namespace CleanArchitecture.Infrastructure;

//Esta clase es internal y sealed porque no se necesita acceder a ella desde fuera de la infraestructura
//y no se necesita heredar de ella
//El agente que se encarga de hacer las conexiones es la interfaz IDateTimeProvider
//Esta clase implementa la interfaz IDateTimeProvider y no necesita ser publica
internal sealed class DateTimeProvider : IDateTimeProvider
{    
    public DateTime CurrentTime => DateTime.UtcNow;
}
