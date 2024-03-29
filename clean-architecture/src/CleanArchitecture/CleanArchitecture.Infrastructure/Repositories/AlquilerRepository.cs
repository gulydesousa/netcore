using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Vehiculos;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories;

internal sealed class AlquilerRepository : Repository<Alquiler>, IAlquilerRepository
{
    private static AlquilerStatus[] statusNoDisponible =
    new AlquilerStatus[] {  AlquilerStatus.Reservado,
                            AlquilerStatus.Confirmado,
                            AlquilerStatus.Completado };

    public AlquilerRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    //Este metodo se encarga de verificar si un vehiculo esta siendo alquilado en un rango de fechas
    //Perteneciente a la interfaz IAlquilerRepository
    //No esta en Repository<T> ya que es un metodo personalizado
    public async Task<bool> IsOverlappingAsync(
        Vehiculo vehiculo,
        DateRange duracion,
        CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Alquiler>()
            .AnyAsync(x => x.VehiculoId == vehiculo.Id &&
                            x.Duracion != null &&
                            x.Duracion.Start <= x.Duracion.End &&
                            x.Duracion.End >= duracion.Start &&
                            statusNoDisponible.Contains(x.Status)
                            , cancellationToken);
    }

}