namespace CleanArchitecture.Api.Shared.ApiDateRange
{
    internal class ApiDateRangeDateOnly : IApiDateRange<DateOnly>
    {
        public DateOnly FechaInicio { get; init; }
        public DateOnly FechaFin { get; init; }


        public ApiDateRangeDateOnly(ApiDateRangeRequest request)
        {
            FechaInicio = DateOnly.Parse(request.FechaInicio);
            FechaFin = DateOnly.Parse(request.FechaFin); 
        }

        //Constructor con dos strings
        public ApiDateRangeDateOnly(string startDate, string endDate)
        {
            FechaInicio = DateOnly.Parse(startDate);
            FechaFin = DateOnly.Parse(endDate);
        }
    }

}
