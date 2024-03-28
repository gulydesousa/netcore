namespace CleanArchitecture.Application.Alquileres.GetAlquiler;

//Sealed porque no se necesita heredar de esta clase
//Usa tipos de datos primitivos para evitar problemas de serialización
//Cuando se usa Dapper, se necesita que las propiedades sean públicas y tengan un constructor sin parámetros
//Se usa init para que las propiedades solo se puedan establecer en el constructor
//Se usa un constructor sin parámetros para que Dapper pueda instanciar la clase
public sealed class AlquilerResponse
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid VehiculoId { get; init; }
    public int Status { get; init; }
    public decimal PrecioAlquiler { get; init; }
    public string? TipoMonedaAlquiler { get; init; }
    public decimal PrecioMantenimiento { get; init; }
    public string? TipoMonedaMantenimiento { get; init; }
    public decimal PrecioAccesorios { get; init; }
    public string? TipoMonedaAccesorios { get; init; }
    public decimal PrecioTotal { get; init; }
    public string? TipoMonedaTotal { get; init; }
    public DateOnly DuracionInicio { get; init; }
    public DateOnly DuracionFinal { get; init; }
    public DateTime FechaCreacion { get; init; }
}
