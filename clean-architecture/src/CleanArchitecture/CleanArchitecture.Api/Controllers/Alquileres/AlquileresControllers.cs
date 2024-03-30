using CleanArchitecture.Api.Shared.ApiDateRange;
using CleanArchitecture.Application.Alquileres.GetAlquiler;
using CleanArchitecture.Application.Alquileres.ReservarAlquiler;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers.Alquileres;

[ApiController]
[Route("api/[controller]")]
public class AlquileresController : ControllerBase
{
    private readonly ISender _sender;

    public AlquileresController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAlquiler(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetAlquilerQuery(id);
        var response = await _sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound();
    }

    [HttpPost("ReservarAlquiler")]
    public async Task<IActionResult> ReservarAlquiler(
        AlquilerReservarRequest request,
        CancellationToken cancellationToken)
    {
        #region validar el rango de fechas
        ApiDateRangeRequest dateRequest = new ApiDateRangeRequest(request);
        var validator = new ApiDateRangeRequestDateOnlyValidator();
        var validationResult = validator.Validate(dateRequest);

        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var dateRange = new ApiDateRangeDateOnly(dateRequest);
        #endregion validar el rango de fechas

        var query = new ReservarAlquilerCommand(
            request.VehiculoId,
            request.UserId,
            dateRange.FechaInicio, 
            dateRange.FechaFin);

        var response = await _sender.Send(query, cancellationToken);

        if (response.IsFailure)
        {
            return BadRequest(response.Error);
        }

        return CreatedAtAction(nameof(GetAlquiler),
                                new { id = response.Value },
                                response.Value);
    }
}
