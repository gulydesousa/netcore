namespace CleanArchitecture.Domain.Vehiculos;

public record Direccion
{
    public string? Pais { get; init; }
    public string? Departamento { get; init; }
    public string? Provincia { get; init; }
    public string? Ciudad { get; init; }
    public string? Calle { get; init; }
}
