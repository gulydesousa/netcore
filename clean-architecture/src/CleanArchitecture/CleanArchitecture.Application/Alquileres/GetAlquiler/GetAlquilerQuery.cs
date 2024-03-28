
using CleanArchitecture.Application.Abstractions.Messaging;

namespace CleanArchitecture.Application.Alquileres.GetAlquiler;

//Implement IQuery interface with AlquilerResponse as the generic type
//Se usa para obtener un alquiler por su Id
public sealed record GetAlquilerQuery(Guid AlquilerId) : IQuery<AlquilerResponse>;

