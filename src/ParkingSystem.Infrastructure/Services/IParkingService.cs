using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingSystem.Core.DTOs;

namespace ParkingSystem.Infrastructure.Services
{
    public interface IParkingService
{
    Task<VehicleViewModel> ParkVehicleAsync(ParkVehicleRequest request);
    Task<ParkingTicketViewModel> RemoveVehicleAsync(string licensePlate);
    Task<IEnumerable<VehicleViewModel>> GetParkedVehiclesAsync();
}
}