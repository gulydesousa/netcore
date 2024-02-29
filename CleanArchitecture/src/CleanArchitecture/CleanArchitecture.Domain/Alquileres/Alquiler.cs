using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres.Events;
using CleanArchitecture.Domain.Vehiculos;
using CleanArchitecture.Domain.Shared;

namespace CleanArchitecture.Domain.Alquileres;

public sealed class Alquiler : Entity
{
    public Guid VehiculoId { get; private set; }

    public AlquilerStatus Status { get; private set; }
    public DateRange? Duracion { get; private set; }
    public Guid? ClienteId { get; private set; }
    public Moneda? PrecioPorPeriodo { get; private set; }
    public Moneda? Mantenimiento { get; private set; }
    public Moneda? Accesorios { get; private set; }
    public Moneda? PrecioTotal { get; private set; }

    public DateTime? FechaCreacion { get; private set; }
    public DateTime? FechaConfirmacion { get; private set; }
    public DateTime? FechaDenegacion { get; private set; }
    public DateTime? FechaCompletado { get; private set; }
    public DateTime? FechaCancelacion { get; private set; }

    public static Alquiler Reservar(Vehiculo vehiculo, DateRange duracion
                                  , Guid clienteId, DateTime fechaCreacion
                                  , PrecioService precioSvc)
    {
        //Se crea un nuevo alquiler con el estado reservado
        //Nos faltan los calculos de los precios, mantenimiento, accesorios y precio total
        //Para no romper el DDD se deja a cargo de un servicio que se encargue de calcular los precios         
        var precioDetalle = precioSvc.CalcularPrecio(vehiculo, duracion);
          
        var alquiler = new Alquiler(Guid.NewGuid()
                                , vehiculo.Id!
                                , AlquilerStatus.Reservado
                                , duracion
                                , clienteId
                                , precioDetalle.PrecioPorPeriodo
                                , precioDetalle.Mantenimiento
                                , precioDetalle.Accesorios
                                , precioDetalle.PrecioTotal
                                , fechaCreacion);

        alquiler.RaiseDomainEvent(new AlquilerReservadoDomainEvent(alquiler.Id!));

        vehiculo.FechaUltimoAlquiler = fechaCreacion;

        return alquiler;
    }
    
    //Un vehiculo puede transitar por varios estados, por lo que se crea una lista de estados
    private Alquiler(Guid id
    , Guid vehiculoId
    , AlquilerStatus status
    , DateRange? duracion
    , Guid? clienteId
    , Moneda? precioPorPeriodo
    , Moneda? mantenimiento
    , Moneda? accesorios
    , Moneda? precioTotal
    , DateTime? fechaCreacion): base(id)
    {
        VehiculoId = vehiculoId;
        Status = status;
        Duracion = duracion;
        ClienteId = clienteId;
        PrecioPorPeriodo = precioPorPeriodo;
        Mantenimiento = mantenimiento;
        Accesorios = accesorios;
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
        
        if(Duracion != null && currentDate.CompareTo(Duracion.Start) > 0)
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
