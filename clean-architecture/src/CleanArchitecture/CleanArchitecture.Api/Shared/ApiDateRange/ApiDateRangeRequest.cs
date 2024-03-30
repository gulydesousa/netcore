using CleanArchitecture.Api.Controllers.Alquileres;

namespace CleanArchitecture.Api.Shared.ApiDateRange
{
    internal class ApiDateRangeRequest
    {
        public string FechaInicio { get; init; } = string.Empty;
        public string FechaFin { get; init; } = string.Empty;


        //Constructor con dos strings
        public ApiDateRangeRequest(string startDate, string endDate)
        {
            FechaInicio = startDate;
            FechaFin = endDate;
        }

        public ApiDateRangeRequest(AlquilerReservarRequest request)
        {
            FechaInicio = request.FechaInicio;
            FechaFin = request.FechaFin;
        }        
    }
}
