using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingSystem.Desktop.Models;

namespace ParkingSystem.Desktop.Services
{
    public interface IParkingApiService
    {
        Task<List<ParkingSpotDto>> GetParkingSpotsAsync();
        // Futuramente:
        // Task<VehicleDto> ParkVehicleAsync(ParkVehicleRequest request);
        // Task<TicketDto> RemoveVehicleAsync(string licensePlate);
    }
}