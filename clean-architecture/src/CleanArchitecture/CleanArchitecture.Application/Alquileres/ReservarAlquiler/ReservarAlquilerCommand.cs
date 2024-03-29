using CleanArchitecture.Application.Abstractions.Messaging;

namespace CleanArchitecture.Application.Alquileres.ReservarAlquiler;

/// <summary>
/// Represents a command to reserve a rental.
/// Este command retorna el id de la reserva realizada ICommand<Guid>.
/// Es un record de C# 9, es inmutable y se usa para definir un comando que se envía a través de la aplicación.
/// En este caso, se usa para reservar un alquiler.
/// Se implementa la interfaz ICommand<T> donde T es el tipo de identificador del alquiler.
/// </summary>
/// 
public record ReservarAlquilerCommand(
    Guid vehiculoId,
    Guid userId,
    DateTime fechaInicio,
    DateTime fechaFin
): ICommand<Guid>;