using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres.Events;
using CleanArchitecture.Domain.Vehiculos;
using CleanArchitecture.Domain.Shared;

namespace CleanArchitecture.Domain.Alquileres;

public sealed class Alquiler : Entity
{
    private Alquiler() { }
    
    public Guid VehiculoId { get; private set; }

    public AlquilerStatus Status { get; private set; }
    public DateRange? Duracion { get; private set; }
    public Guid? UserId { get; private set; }
    public Moneda? PrecioPorPeriodo { get; private set; }
    public Moneda? PrecioMantenimiento { get; private set; }
    public Moneda? PrecioAccesorios { get; private set; }
    //Sera la suma de los precios anteriores: 
    //PrecioPorPeriodo + PrecioMantenimiento + PrecioAccesorios
    public Moneda? PrecioTotal { get; private set; }
    public DateTime? FechaCreacion { get; private set; }
    //Fecha en la que el usuario dice sí voy a alquilar el coche
    public DateTime? FechaConfirmacion { get; private set; }
    //Fecha en la que el usuario dice no voy a alquilar el coche
    public DateTime? FechaDenegacion { get; private set; }
    //Fecha en la que se ha completado toda la transacción
    public DateTime? FechaCompletado { get; private set; }
    //A pesar que ya pagué, no puedo alquilar el coche
    public DateTime? FechaCancelacion { get; private set; }

    public static Alquiler Reservar(Vehiculo vehiculo
                                 , Guid userId
                                 , DateRange duracion
                                 , DateTime fechaCreacion
                                 , PrecioService precioService)
    {
       
        var precioDetalle = precioService.CalcularPrecio(vehiculo, duracion);


        var alquiler = new Alquiler(Guid.NewGuid()
                                , vehiculo.Id!
                                , AlquilerStatus.Reservado
                                , duracion
                                , userId
                                , precioDetalle.PrecioPorPeriodo
                                , precioDetalle.Mantenimiento
                                , precioDetalle.Accesorios
                                , precioDetalle.PrecioTotal
                                , fechaCreacion);
        //Se dispara un evento de dominio para notificar que el alquiler ha sido reservado
        //Esperando que a futuro alguien se suscriba a este evento y realice alguna acción
        //Por ejemplo, enviar un correo electrónico al usuario o al propietario del vehículo
        //¿Quien esta suscrito a este evento? 
        alquiler.RaiseDomainEvent(new AlquilerReservadoDomainEvent(alquiler.Id!));

        //Fecha de creacion es la fecha del ultimo alquiler
        vehiculo.FechaUltimoAlquiler = fechaCreacion;

        return alquiler;
    }

    //Un vehiculo puede transitar por varios estados
    //, por lo que se crea una lista de estados
    private Alquiler(Guid id
    , Guid vehiculoId
    , AlquilerStatus status
    , DateRange? duracion
    , Guid? userId
    , Moneda? precioPorPeriodo
    , Moneda? precioMantenimiento
    , Moneda? precioAccesorios
    , Moneda? precioTotal
    , DateTime? fechaCreacion) : base(id)
    {
        VehiculoId = vehiculoId;
        Status = status;
        Duracion = duracion;
        UserId = userId;
        PrecioPorPeriodo = precioPorPeriodo;
        PrecioMantenimiento = precioMantenimiento;
        PrecioAccesorios = precioAccesorios;
        PrecioTotal = precioTotal;
        FechaCreacion = fechaCreacion;
    }

    public Result Confirmar(DateTime utcNow)
    {
        if (Status != AlquilerStatus.Reservado)
        {
            //Se disparará un evento de dominio para notificar que el alquiler no se puede confirmar
            return Result.Failure(AlquilerErrors.NotReserved);
        }

        Status = AlquilerStatus.Confirmado;
        FechaConfirmacion = utcNow;

        RaiseDomainEvent(new AlquilerConfirmadoDomainEvent(Id!));
        return Result.Success();
    }

    public Result Rechazar(DateTime utcNow)
    {
        if (Status != AlquilerStatus.Reservado)
        {
            //Se disparará un evento de dominio para notificar que el alquiler no se puede denegar
            return Result.Failure(AlquilerErrors.NotReserved);
        }

        Status = AlquilerStatus.Rechazar;
        FechaDenegacion = utcNow;

        RaiseDomainEvent(new AlquilerRechazadoDomainEvent(Id!));
        return Result.Success();
    }

    public Result Cancelar(DateTime utcNow)
    {
        if (Status == AlquilerStatus.Confirmado)
        {
            return Result.Failure(AlquilerErrors.NotConfirmed);
        }

        var currentDate = DateTime.UtcNow.Date;

        if (Duracion != null && currentDate.CompareTo(Duracion.Start) > 0)
        {
            return Result.Failure(AlquilerErrors.AlreadyStarted);
        }

        Status = AlquilerStatus.Cancelado;
        FechaCancelacion = utcNow;
        RaiseDomainEvent(new AlquilerCanceladoDomainEvent(Id!));

        return Result.Success();
    }


    public Result Completar(DateTime utcNow)
    {
        if (Status != AlquilerStatus.Confirmado)
        {
            //Se disparará un evento de dominio para notificar que el alquiler no se puede denegar
            return Result.Failure(AlquilerErrors.NotConfirmed);
        }

        Status = AlquilerStatus.Completado;
        FechaCompletado = utcNow;

        RaiseDomainEvent(new AlquilerCompletadoDomainEvent(Id!));
        return Result.Success();
    }

}
