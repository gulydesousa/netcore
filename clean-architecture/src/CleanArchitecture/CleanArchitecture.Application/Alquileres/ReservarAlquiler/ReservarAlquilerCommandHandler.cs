using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Application.Abstractions.Clock;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehiculos;

namespace CleanArchitecture.Application.Alquileres.ReservarAlquiler;

internal sealed class ReservarAlquilerCommandHandler : ICommandHandler<ReservarAlquilerCommand, Guid>
{
    //Se inyectan las dependencias necesarias para el manejador de comandos.
    private readonly IUserRepository _userRepository;
    private readonly IVehiculoRepository _vehiculoRepository;
    private readonly IAlquilerRepository _alquilerRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly PrecioService _precioService;

    //Se inyecta la unitOfWork para guardar los cambios en la base de datos.
    //unitOfWork es una clase que se usa para agrupar todas las operaciones
    //de base de datos en una sola transacción.
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Handles the command for reserving an alquiler (rental).
    /// </summary>
    public ReservarAlquilerCommandHandler(
        IUserRepository userRepository,
        IVehiculoRepository vehiculoRepository,
        IAlquilerRepository alquilerRepository,
        IDateTimeProvider dateTimeProvider,
        PrecioService precioService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _vehiculoRepository = vehiculoRepository;
        _alquilerRepository = alquilerRepository;
        _dateTimeProvider = dateTimeProvider;
        _precioService = precioService;
        _unitOfWork = unitOfWork;
    }
    
    /// <summary>
    /// Handles the ReservarAlquilerCommand by processing the request and performing the necessary operations.
    /// </summary>
    /// <param name="request">The ReservarAlquilerCommand to handle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result 
    /// contains the result of the operation, which is a Result object containing a Guid.
    /// </returns>
    public async Task<Result<Guid>> Handle(
        ReservarAlquilerCommand request
      , CancellationToken cancellationToken)
    {
       
       //Se busca el usuario por el id proporcionado en el comando.
        var user = await _userRepository.GetByIdAsync(request.userId, cancellationToken);
        
        if (user is null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound);
        }
         
        //Se busca el vehículo por el id proporcionado en el comando.
        var vehiculo = await _vehiculoRepository.GetByIdAsync(request.vehiculoId, cancellationToken);

        if (vehiculo is null)
        {
            return Result.Failure<Guid>(VehiculoErrors.NotFound);
        }
        
        //Se crea un objeto DateRange con las fechas de inicio y fin proporcionadas en el comando.
        var duracion = DateRange.Create(request.fechaInicio, request.fechaFin);
        
        //Se verifica si hay superposición de fechas para el vehículo proporcionado.
        if(await _alquilerRepository.IsOverlappingAsync(vehiculo, (DateRange)duracion, cancellationToken))
        {
            return Result.Failure<Guid>(AlquilerErrors.Overlap);
        }
       
       //Se reserva el alquiler con los datos proporcionados.
        var alquiler = Alquiler.Reservar(
            vehiculo,
            user.Id,
            (DateRange)duracion,
            _dateTimeProvider.CurrentTime,
            _precioService);
        //Se agrega el alquiler a la base de datos.
        _alquilerRepository.Add(alquiler);
        //Se guardan los cambios en la base de datos.
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        //Se devuelve el id del alquiler reservado.
        return Result.Success(alquiler.Id);
    }
}