using Microsoft.AspNetCore.Mvc;
using ParkingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ParkingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParkingSpotsController : ControllerBase
    {
        private readonly ParkingDbContext _context;

        public ParkingSpotsController(ParkingDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSpots()
        {
            var spots = await _context.ParkingSpots
                .Select(s => new { s.Id, s.Number, s.Type, s.IsOccupied })
                .ToListAsync();
            return Ok(spots);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSpotById(int id)
        {
            var spot = await _context.ParkingSpots
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();

            if (spot == null) 
                return NotFound();

            var currentVehicle = spot.IsOccupied ? await _context.Vehicles
                .Where(v => v.ParkingSpotId == id && v.IsParked)
                .Select(v => new { v.LicensePlate, v.Model, v.EntryTime })
                .FirstOrDefaultAsync() : null;

            var result = new
            {
                spot.Id,
                spot.Number,
                spot.Type,
                spot.IsOccupied,
                CurrentVehicle = currentVehicle
            };

            return Ok(result);
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetOccupancyStats()
        {
            var totalSpots = await _context.ParkingSpots.CountAsync();
            var occupiedSpots = await _context.ParkingSpots.CountAsync(s => s.IsOccupied);

            return Ok(new
            {
                TotalSpots = totalSpots,
                OccupiedSpots = occupiedSpots,
                AvailableSpots = totalSpots - occupiedSpots,
                OccupancyRate = totalSpots > 0 ? (double)occupiedSpots / totalSpots : 0
            });
        }

        [HttpGet("detailed")]
        public async Task<IActionResult> GetSpotsWithDetails()
        {
            var spots = await _context.ParkingSpots.ToListAsync();
            
            var parkedVehicles = await _context.Vehicles
                .Where(v => v.IsParked)
                .ToListAsync();

            var result = spots.Select(spot => new
            {
                spot.Id,
                spot.Number,
                spot.Type,
                spot.IsOccupied,
                CurrentVehicle = parkedVehicles
                    .Where(v => v.ParkingSpotId == spot.Id)
                    .Select(v => new { v.LicensePlate, v.Model, v.EntryTime })
                    .FirstOrDefault()
            }).ToList();

            return Ok(result);
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableSpots()
        {
            var availableSpots = await _context.ParkingSpots
                .Where(s => !s.IsOccupied)
                .Select(s => new { s.Id, s.Number, s.Type })
                .ToListAsync();

            return Ok(availableSpots);
        }

        [HttpGet("occupied")]
        public async Task<IActionResult> GetOccupiedSpots()
        {
            var occupiedSpots = await _context.ParkingSpots
                .Where(s => s.IsOccupied)
                .ToListAsync();

            var spotIds = occupiedSpots.Select(s => s.Id).ToList();
            
            var vehicles = await _context.Vehicles
                .Where(v => v.IsParked && spotIds.Contains(v.ParkingSpotId))
                .ToListAsync();

            var result = occupiedSpots.Select(spot => 
            {
                var vehicle = vehicles.FirstOrDefault(v => v.ParkingSpotId == spot.Id);
                return new
                {
                    spot.Id,
                    spot.Number,
                    spot.Type,
                    spot.IsOccupied,
                    CurrentVehicle = vehicle != null ? new
                    {
                        vehicle.LicensePlate,
                        vehicle.Model,
                        vehicle.Color,
                        vehicle.EntryTime,
                        TimeParked = DateTime.Now - vehicle.EntryTime
                    } : null
                };
            }).ToList();

            return Ok(result);
        }
    }
}