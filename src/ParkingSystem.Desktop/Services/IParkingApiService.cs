using ParkingSystem.Desktop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using ParkingSystem.Core.DTOs;

namespace ParkingSystem.Desktop.Services
{
    public interface IParkingApiService
    {
        Task<List<ParkingSpotDto>> GetParkingSpotsAsync();
        Task<VehicleViewModel?> GetVehicleInSpotAsync(int spotId);
        Task<VehicleViewModel?> ParkVehicleAsync(ParkVehicleRequest request);
        Task<ParkingTicketViewModel?> RemoveVehicleAsync(string licensePlate);
    }
}