using FluentValidation;
using System.Globalization;

namespace CleanArchitecture.Api.Shared.ApiDateRange;

internal class ApiDateRangeRequestDateOnlyValidator : AbstractValidator<ApiDateRangeRequest>
{
    public ApiDateRangeRequestDateOnlyValidator()
    {
        RuleFor(x => x.FechaInicio).NotEmpty().Must(BeValidDateFormat).WithMessage("Invalid date format. Please use 'yyyy-MM-dd'.");
        RuleFor(x => x.FechaFin).NotEmpty().Must(BeValidDateFormat).WithMessage("Invalid date format. Please use 'yyyy-MM-dd'.");

        RuleFor(x => x).Custom((request, context) =>
        {
            if (DateTime.Parse(request.FechaInicio) >= DateTime.Parse(request.FechaFin))
            {
                context.AddFailure("Start date must be before end date");
            }
        });
    }

    private bool BeValidDateFormat(string date)
    {
        bool result;
        result = DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        return result;
    }

}