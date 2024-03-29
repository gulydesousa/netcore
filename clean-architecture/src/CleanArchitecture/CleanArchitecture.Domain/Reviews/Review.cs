using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Reviews.Events;

namespace CleanArchitecture.Domain.Reviews;

public sealed class Review: Entity
{
    private Review() { }
    
    public Guid VehiculoId { get; private set; }
    public Guid AlquilerId { get; private set; } 
    public Guid UserId { get; private set; }
    public Rating Rating { get; private set; }
    public Comentario Comentario { get; private set; }
    public DateTime FechaCreacion { get; private set; }

    private Review(Guid id
    , Guid vehiculoId
    , Guid alquilerId
    , Guid userId
    , Rating rating
    , Comentario comentario
    , DateTime fechaCreacion) : base(id)
    {
        VehiculoId = vehiculoId;
        AlquilerId = alquilerId;
        UserId = userId;
        Rating = rating;
        Comentario = comentario;
        FechaCreacion = fechaCreacion;
    }

    public static Result<Review> Create(
     Alquiler alquiler,
     Rating rating,
     Comentario comentario,
        DateTime fechaCreacion)
    
    {
        if(alquiler.Status != AlquilerStatus.Completado)
        {
            return Result.Failure<Review>(ReviewErrors.NotElegible);
        }

        var review = new Review(
        Guid.NewGuid(),
        alquiler.VehiculoId,
        alquiler.Id,
        alquiler.UserId ?? Guid.Empty,
        rating,
        comentario,
        fechaCreacion);

        review.RaiseDomainEvent(new ReviewCreatedDomainEvent(review.Id!));

        return Result.Success(review);
    }


}