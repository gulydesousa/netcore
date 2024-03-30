namespace CleanArchitecture.Domain.Alquileres;

public sealed record DateRange
{
    public DateOnly Start { get; init; }
    public DateOnly End { get; init; }

    public int CantidadDias => End.DayNumber - Start.DayNumber;

    private DateRange(DateOnly start, DateOnly end)
    {
        Start = start;
        End = end;
    }


    public static DateRange Create(DateOnly start, DateOnly end)
    {
        if (start > end)
        {
            throw new ApplicationException("Start date must be before end date");
        }

        return new DateRange(start, end);
    }

    public static DateRange Create(string start, string end)
    {
        if (!DateOnly.TryParse(start, out DateOnly parsedStart) ||
            !DateOnly.TryParse(end, out DateOnly parsedEnd))
        {
            throw new FormatException("Invalid date format. Please use 'yyyy-MM-dd'.");
        }

        return Create(parsedStart, parsedEnd);
    }



    public static object Create(DateTime fechaInicio, DateTime fechaFin)
    {
        throw new NotImplementedException();
    }
}