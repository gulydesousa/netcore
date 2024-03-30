using System.Globalization;

namespace CleanArchitecture.Api.Extensions;

public static class StringExtensions
{
    public static DateTime ToDateTime(this string date)
    {
        if (!DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
        {
            throw new FormatException("Invalid date format. Please use 'yyyy-MM-dd'.");
        }

        return parsedDate;
    }

    public static DateOnly ToDateOnly(this string date)
    {
        if (!DateOnly.TryParse(date, out DateOnly parsedDate))
        {
            throw new FormatException("Invalid date format. Please use 'yyyy-MM-dd'.");
        }

        return parsedDate;
    }
}