namespace CleanArchitecture.Domain.Shared;

public record TipoMoneda
{
    public static TipoMoneda USD = new("USD");
    public static TipoMoneda EUR = new("EUR");
    public static TipoMoneda None = new("");

    public string? Codigo { get; init; }

    private TipoMoneda(string codigo) => Codigo = codigo;

    /// <summary>
    /// Lista de todas las monedas
    /// </summary>
    public static readonly IReadOnlyCollection<TipoMoneda> All =
    [
        USD,
        EUR,
    ];

    /// <summary>
    /// Obtiene la moneda por su código
    /// </summary>
    /// <param name="codigo"></param>
    /// <returns></returns>
    public static TipoMoneda FromCodigo(string codigo) =>
        All.FirstOrDefault(x => x.Codigo == codigo) 
        ?? throw new InvalidOperationException($"No se encontró la moneda con el código {codigo}");
}
