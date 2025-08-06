using Microsoft.AspNetCore.Mvc;
using ParkingSystem.Core.DTOs;
using ParkingSystem.Infrastructure.Services;

namespace ParkingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
    private readonly IParkingService _parkingService;

    public VehiclesController(IParkingService parkingService)
    {
        _parkingService = parkingService;
    }

    [HttpPost("park")]
    [ProducesResponseType(typeof(VehicleViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ParkVehicle([FromBody] ParkVehicleRequest request)
    {
        try
        {
            var parkedVehicle = await _parkingService.ParkVehicleAsync(request);
            return Ok(parkedVehicle);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception)
            {
            return StatusCode(500, "Ocorreu um erro interno ao tentar estacionar o veículo.");
        }
    }

    [HttpPost("exit/{licensePlate}")]
    [ProducesResponseType(typeof(ParkingTicketViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ExitVehicle(string licensePlate)
    {
        try
        {
            var ticket = await _parkingService.RemoveVehicleAsync(licensePlate);
            return Ok(ticket);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, "Ocorreu um erro interno ao tentar remover o veículo.");
        }
    }
    
    [HttpGet("parked")]
    [ProducesResponseType(typeof(IEnumerable<VehicleViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetParkedVehicles()
    {
        var vehicles = await _parkingService.GetParkedVehiclesAsync();
        return Ok(vehicles);
    }
}
}