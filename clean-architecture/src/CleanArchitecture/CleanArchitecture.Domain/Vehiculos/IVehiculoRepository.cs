namespace CleanArchitecture.Domain.Vehiculos;
    public interface IVehiculoRepository
    {
        //Método que agrega un vehículo a la base de datos
        //CancelationToken: permite cancelar una operación asincrónica
        Task<Vehiculo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        void Add(Vehiculo vehiculo);
    }
