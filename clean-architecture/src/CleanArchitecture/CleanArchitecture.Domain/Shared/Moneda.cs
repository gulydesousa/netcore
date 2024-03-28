namespace CleanArchitecture.Domain.Shared;

public record Moneda(decimal Monto, TipoMoneda TipoMoneda)
{
    /// <summary>
    /// Suma dos montos de la misma moneda
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static Moneda operator +(Moneda a, Moneda b)
    {
        if (a.TipoMoneda != b.TipoMoneda)
        {
            throw new InvalidOperationException("No se pueden sumar montos de diferentes monedas");
        }

        return new Moneda(a.Monto + b.Monto, a.TipoMoneda);
    }

    public static Moneda Zero() => new Moneda(0, TipoMoneda.None);
    public static Moneda Zero(TipoMoneda tipoMoneda) => new Moneda(0, tipoMoneda);
    public bool IsZero() => this == Zero();
    

    /// <summary>
    /// Resta dos montos de la misma moneda
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static Moneda operator -(Moneda a, Moneda b)
    {
        if (a.TipoMoneda != b.TipoMoneda)
        {
            throw new InvalidOperationException("No se pueden restar montos de diferentes monedas");
        }

        return new Moneda(a.Monto - b.Monto, a.TipoMoneda);
    }

    /// <summary>
    /// Multiplica un monto por un factor
    /// </summary>
    /// <param name="a"></param>
    /// <param name="factor"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static Moneda operator *(Moneda a, decimal factor)
    {
        return new Moneda(a.Monto * factor, a.TipoMoneda);
    }
    
    /// <summary>
    /// Divide un monto por un factor
    /// </summary>
    /// <param name="a"></param>
    /// <param name="factor"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static Moneda operator /(Moneda a, decimal factor)
    {
        return new Moneda(a.Monto / factor, a.TipoMoneda);
    }
    
}