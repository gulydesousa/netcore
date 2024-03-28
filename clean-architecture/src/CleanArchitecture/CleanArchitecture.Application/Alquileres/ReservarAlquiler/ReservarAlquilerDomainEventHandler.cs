using CleanArchitecture.Application.Abstractions.Email;
using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Alquileres.Events;
using CleanArchitecture.Domain.Users;
using MediatR;

namespace CleanArchitecture.Application.Alquileres.ReservarAlquiler;

//Se crea una clase que representa un manejador de eventos de dominio para 
//el evento de dominio AlquilerReservadoDomainEvent.
//Un manejador de eventos de dominio es una clase que procesa un evento de dominio.
//Un evento de dominio es una clase que representa un evento que ha ocurrido en el dominio.
//En este caso, el evento de dominio AlquilerReservadoDomainEvent representa un alquiler 
//que ha sido reservado.
//El manejador de eventos de dominio ReservarAlquilerDomainEventHandler procesa el 
//evento de dominio AlquilerReservadoDomainEvent.
//Se usa para realizar alguna acción cuando se produce el evento de dominio AlquilerReservadoDomainEvent.
//Por ejemplo, enviar un correo electrónico al usuario o al propietario del vehículo.
//Se implementa la interfaz INotificationHandler<T> de MediatR.
//INotificationHandler<T> es una interfaz de MediatR que representa un manejador de notificaciones.
//Se usa para definir un manejador de notificaciones. Y permite que una clase maneje una notificación.
//Es internal y sealed porque solo se usa dentro de la aplicación y no se puede heredar.
//INotificacionHandler<T> es una interfaz de Mediat y le dice al mundo "Yo escucho todo tipo de eventos"
//"¿Que evento quieres que escuche para ti?" Yo quiero que escuches el evento AlquilerReservadoDomainEvent
//AlquilerReservadoDomainEvent es un evento de dominio que representa un alquiler que ha sido reservado.
internal sealed class ReservarAlquilerDomainEventHandler
: INotificationHandler<AlquilerReservadoDomainEvent>
{
    //Se inyectan las dependencias necesarias para el manejador de eventos de dominio.
    private readonly IAlquilerRepository _alquilerRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;

    public ReservarAlquilerDomainEventHandler(
      IAlquilerRepository alquilerRepository
    , IUserRepository userRepository
    , IEmailService emailService)
    {
        _alquilerRepository = alquilerRepository;
        _userRepository = userRepository;
        _emailService = emailService;
    }

    public async Task Handle(
        AlquilerReservadoDomainEvent notification
      , CancellationToken cancellationToken)
    {
        //Se realiza alguna acción cuando se produce el evento de dominio AlquilerReservadoDomainEvent.
        //Por ejemplo, enviar un correo electrónico al usuario o al propietario del vehículo.
        //Se comprueba que existe un alquiler para enviar el correo
        var alquiler = await _alquilerRepository.GetByIdAsync(notification.AlquilerId, cancellationToken);
        if (alquiler is null || !alquiler.UserId.HasValue)  return;

        //Se busca el usuario por el id en el alquiler.
        var user = await _userRepository.GetByIdAsync((Guid)alquiler.UserId, cancellationToken);
        if (user is null || user.Email is null) return;

        //Se envía un correo electrónico al usuario para notificarle que el alquiler ha sido reservado.
        await _emailService.SendEmailAsync(
            user.Email
          , "Alquiler reservado"
          , $"Tienes que confirmar esta reserva, de lo contrario se va a perder.");
    }
}
