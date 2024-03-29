namespace CleanArchitecture.Api.Controllers.Alquileres;

public sealed record AlquilerReservaRequest(
    Guid VehiculoId,
    Guid UserId,
    DateOnly FechaInicio,
    DateOnly FechaFin);