using CleanArchitecture.Application.Vehiculos.SearchVehiculos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers.Vehiculos;

[ApiController]
[Route("api/[controller]")]
public class VehiculosController: ControllerBase
{
    private readonly ISender _sender;

    public VehiculosController(ISender sender)
    {
        _sender = sender;
    }

    /*
    [HttpGet]
    public async Task<IActionResult> SearchVehiculos(
        DateOnly startDate, 
        DateOnly endDate, 
        CancellationToken cancellationToken)
    {
        var query = new SearchVehiculosQuery(startDate, endDate);
        var response = await _sender.Send(query, cancellationToken);                
        return Ok(response.Value);
    }
    */

    [HttpGet]
    public async Task<IActionResult> SearchVehiculos(
    string startDate,
    string endDate,
    CancellationToken cancellationToken)
    {
        DateTime dateStart;
        DateTime dateEnd;
        if (!DateTime.TryParse(startDate, out dateStart) ||
            !DateTime.TryParse(endDate, out dateEnd))
        {
            return BadRequest("Invalid date format. Please use 'yyyy-MM-dd'.");
        }
        
        var query = new SearchVehiculosQuery(
            DateOnly.FromDateTime(dateStart), 
            DateOnly.FromDateTime(dateEnd));
        var response = await _sender.Send(query, cancellationToken);
        return Ok(response.Value);
    }
}

