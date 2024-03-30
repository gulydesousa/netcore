using CleanArchitecture.Api.Shared.ApiDateRange;
using CleanArchitecture.Application.Vehiculos.SearchVehiculos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers.Vehiculos;

[ApiController]
[Route("api/[controller]")]
public class VehiculosController : ControllerBase
{
    private readonly ISender _sender;

    public VehiculosController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> SearchVehiculos(
        string startDate,
        string endDate,
    CancellationToken cancellationToken)
    {
        #region validar el rango de fechas
        ApiDateRangeRequest request = new ApiDateRangeRequest(startDate, endDate);
        var validator = new ApiDateRangeRequestDateOnlyValidator();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var dateRange = new ApiDateRangeDateOnly(request);
        #endregion validar el rango de fechas

        var query = new SearchVehiculosQuery(dateRange.FechaInicio, dateRange.FechaFin);
        var response = await _sender.Send(query, cancellationToken);
        return Ok(response.Value);
    }
}