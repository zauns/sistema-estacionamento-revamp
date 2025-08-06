using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingSystem.Infrastructure.Data;
using ParkingSystem.Core.Entities;
using ParkingSystem.Core.Enums;

namespace ParkingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ParkingDbContext _context;

        public TestController(ParkingDbContext context)
        {
            _context = context;
        }

        // GET: api/test/health
        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new
            {
                Status = "API está funcionando!",
                Timestamp = DateTime.Now,
                Version = "1.0.0",
                Database = "SQLite"
            });
        }

        // GET: api/test/connection
        [HttpGet("connection")]
        public async Task<IActionResult> TestDatabaseConnection()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                var spotsCount = await _context.ParkingSpots.CountAsync();
                
                return Ok(new
                {
                    CanConnect = canConnect,
                    Message = "Conexão com banco de dados OK!",
                    TotalSpots = spotsCount,
                    DatabasePath = "parking_system.db"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Error = "Erro na conexão com banco",
                    Details = ex.Message
                });
            }
        }

        // GET: api/test/spots
        [HttpGet("spots")]
        public async Task<IActionResult> GetAllSpots()
        {
            try
            {
                var spots = await _context.ParkingSpots
                    .Include(s => s.VehicleHistory.Where(v => v.IsParked))
                    .Select(s => new
                    {
                        s.Id,
                        s.Number,
                        s.Type,
                        s.IsOccupied,
                        CurrentVehicle = s.VehicleHistory
                            .Where(v => v.IsParked)
                            .Select(v => new { v.LicensePlate, v.Model })
                            .FirstOrDefault()
                    })
                    .ToListAsync();

                return Ok(new
                {
                    TotalSpots = spots.Count,
                    OccupiedSpots = spots.Count(s => s.IsOccupied),
                    AvailableSpots = spots.Count(s => !s.IsOccupied),
                    Spots = spots.Take(10) // Primeiras 10 para teste
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        // POST: api/test/park
        [HttpPost("park")]
        public async Task<IActionResult> ParkVehicle([FromBody] TestParkRequest request)
        {
            try
            {
                // Validações básicas
                if (string.IsNullOrWhiteSpace(request.LicensePlate))
                    return BadRequest("Placa é obrigatória");

                if (request.SpotId <= 0)
                    return BadRequest("ID da vaga inválido");

                // Verificar se vaga existe e está disponível
                var spot = await _context.ParkingSpots
                    .Include(s => s.VehicleHistory)
                    .FirstOrDefaultAsync(s => s.Id == request.SpotId);

                if (spot == null)
                    return NotFound($"Vaga {request.SpotId} não encontrada");

                if (spot.IsOccupied)
                    return Conflict($"Vaga {spot.Number} já está ocupada");

                // Verificar se veículo já não está estacionado
                var existingVehicle = await _context.Vehicles
                    .FirstOrDefaultAsync(v => v.LicensePlate == request.LicensePlate && v.IsParked);

                if (existingVehicle != null)
                    return Conflict($"Veículo {request.LicensePlate} já está estacionado na vaga {existingVehicle.ParkingSpotId}");

                // Criar novo registro de veículo
                var vehicle = new Vehicle
                {
                    LicensePlate = request.LicensePlate.ToUpper(),
                    Model = request.Model ?? "Não informado",
                    Color = request.Color ?? "Não informado",
                    EntryTime = DateTime.Now,
                    ParkingSpotId = request.SpotId,
                    IsParked = true,
                    TotalAmount = 0
                };

                _context.Vehicles.Add(vehicle);
                
                // Atualizar status da vaga
                spot.IsOccupied = true;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Veículo estacionado com sucesso!",
                    Vehicle = new
                    {
                        vehicle.Id,
                        vehicle.LicensePlate,
                        vehicle.Model,
                        vehicle.Color,
                        vehicle.EntryTime
                    },
                    Spot = new
                    {
                        spot.Id,
                        spot.Number,
                        spot.Type
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        // DELETE: api/test/remove/{licensePlate}
        [HttpDelete("remove/{licensePlate}")]
        public async Task<IActionResult> RemoveVehicle(string licensePlate)
        {
            try
            {
                var vehicle = await _context.Vehicles
                    .Include(v => v.ParkingSpot)
                    .FirstOrDefaultAsync(v => v.LicensePlate == licensePlate.ToUpper() && v.IsParked);

                if (vehicle == null)
                    return NotFound($"Veículo {licensePlate} não encontrado ou não está estacionado");

                // Calcular tempo e valor
                var timeParked = DateTime.Now - vehicle.EntryTime;
                var hours = Math.Ceiling(timeParked.TotalHours);
                var amount = (decimal)(5.00 + (hours - 1) * 3.00); // R$ 5 inicial + R$ 3 por hora adicional

                // Atualizar veículo
                vehicle.ExitTime = DateTime.Now;
                vehicle.IsParked = false;
                vehicle.TotalAmount = amount;

                // Liberar vaga
                vehicle.ParkingSpot.IsOccupied = false;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Veículo removido com sucesso!",
                    Vehicle = new
                    {
                        vehicle.LicensePlate,
                        vehicle.EntryTime,
                        vehicle.ExitTime,
                        TimeParked = $"{timeParked.Hours}h {timeParked.Minutes}min",
                        TotalAmount = vehicle.TotalAmount.ToString("C")
                    },
                    Spot = new
                    {
                        vehicle.ParkingSpot.Number,
                        Status = "Disponível"
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }

    // DTO para requisição de estacionamento
    public class TestParkRequest
    {
        public string LicensePlate { get; set; } = string.Empty;
        public string? Model { get; set; }
        public string? Color { get; set; }
        public int SpotId { get; set; }
    }
}