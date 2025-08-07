using ParkingSystem.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParkingSystem.Web.Services
{
    public interface IParkingApiService
    {
        Task<List<ParkingSpotDto>> GetParkingSpotsAsync();
        Task<VehicleViewModel?> GetVehicleInSpotAsync(int spotId);
        Task<VehicleViewModel?> ParkVehicleAsync(ParkVehicleRequest request);
        Task<ParkingTicketViewModel?> RemoveVehicleAsync(string licensePlate);
        Task<int[]?> GetTodayEntriesByHourAsync();
    }
}