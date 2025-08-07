using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingSystem.Core.DTOs;
using ParkingSystem.Infrastructure.Data;
using ParkingSystem.Infrastructure.Services;

namespace ParkingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly IParkingService _parkingService;
        private readonly ParkingDbContext _context;

        public VehiclesController(IParkingService parkingService, ParkingDbContext context)
        {
            _parkingService = parkingService;
            _context = context;
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
    [HttpGet("history/today-by-hour")]
    public async Task<IActionResult> GetTodayEntriesByHour()
    {
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        var entries = await _context.Vehicles
            .Where(v => v.EntryTime >= today && v.EntryTime < tomorrow)
            .ToListAsync();

        var entriesByHour = entries
            .GroupBy(v => v.EntryTime.Hour)
            .Select(g => new { Hour = g.Key, Count = g.Count() })
            .OrderBy(x => x.Hour)
            .ToList();

        // Cria um array de 24 posições (para cada hora) e preenche com os dados
        var hourlyCounts = new int[24];
        foreach (var entry in entriesByHour)
        {
            hourlyCounts[entry.Hour] = entry.Count;
        }

        return Ok(hourlyCounts);
    }
}
}