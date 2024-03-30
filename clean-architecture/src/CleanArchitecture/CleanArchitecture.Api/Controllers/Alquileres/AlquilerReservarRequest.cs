namespace CleanArchitecture.Api.Controllers.Alquileres;

public sealed record AlquilerReservarRequest(
    Guid VehiculoId,
    Guid UserId,
    string FechaInicio,
    string FechaFin);