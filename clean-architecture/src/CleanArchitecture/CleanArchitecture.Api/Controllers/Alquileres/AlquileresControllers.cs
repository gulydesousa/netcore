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
        AlquilerReservaRequest request,
        CancellationToken cancellationToken)
    {
        DateTime dateStart;
        DateTime dateEnd;
        if (!DateTime.TryParse(request.FechaInicio, out dateStart) ||
            !DateTime.TryParse(request.FechaFin, out dateEnd))
        {
            return BadRequest("Invalid date format. Please use 'yyyy-MM-dd'.");
        }

        var query = new ReservarAlquilerCommand(       
            request.VehiculoId,
            request.UserId,
            dateStart,
            dateEnd
        ); 
        var response = await _sender.Send(query, cancellationToken);

        if(response.IsFailure)
        {
            return BadRequest(response.Error);
        }
        
        return CreatedAtAction( nameof(GetAlquiler), 
                                new { id = response.Value }, 
                                response.Value);
    }
}
