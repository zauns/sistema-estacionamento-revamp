using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingSystem.Core.DTOs;
using ParkingSystem.Core.Entities;
using ParkingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;



namespace ParkingSystem.API.Services
{
    public class ParkingService : IParkingService
    {
        private readonly ParkingDbContext _context;

        public ParkingService(ParkingDbContext context)
        {
            _context = context;
        }

        public async Task<VehicleViewModel> ParkVehicleAsync(ParkVehicleRequest request)
        {
            var spot = await _context.ParkingSpots.FindAsync(request.SpotId);
            if (spot == null) throw new KeyNotFoundException($"Vaga {request.SpotId} não encontrada.");
            if (spot.IsOccupied) throw new InvalidOperationException($"Vaga {spot.Number} já está ocupada.");

            var existingVehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.LicensePlate == request.LicensePlate.ToUpper() && v.IsParked);
            if (existingVehicle != null) throw new InvalidOperationException($"Veículo {request.LicensePlate} já está estacionado.");

            var vehicle = new Vehicle
            {
                LicensePlate = request.LicensePlate.ToUpper(),
                Model = request.Model ?? "Não informado",
                Color = request.Color ?? "Não informado",
                EntryTime = DateTime.UtcNow, 
                ParkingSpotId = request.SpotId,
                IsParked = true
            };

            spot.IsOccupied = true;
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return new VehicleViewModel
            {
                Id = vehicle.Id,
                LicensePlate = vehicle.LicensePlate,
                Model = vehicle.Model,
                Color = vehicle.Color,
                EntryTime = vehicle.EntryTime,
                ParkingSpotId = spot.Id,
                ParkingSpotNumber = spot.Number
            };
        }

        public async Task<ParkingTicketViewModel> RemoveVehicleAsync(string licensePlate)
        {
            var vehicle = await _context.Vehicles
                .Include(v => v.ParkingSpot)
                .FirstOrDefaultAsync(v => v.LicensePlate == licensePlate.ToUpper() && v.IsParked);

            if (vehicle == null) throw new KeyNotFoundException($"Veículo {licensePlate} não encontrado ou não está estacionado.");

            var exitTime = DateTime.UtcNow;
            var timeParked = exitTime - vehicle.EntryTime;

            const decimal firstHourRate = 5.00m;
            const decimal additionalHourRate = 3.00m;
            var totalHours = Math.Ceiling(timeParked.TotalHours);
            var amount = firstHourRate;
            if (totalHours > 1)
            {
                amount += ((decimal)totalHours - 1) * additionalHourRate;
            }

            vehicle.ExitTime = exitTime;
            vehicle.IsParked = false;
            vehicle.TotalAmount = amount;
            vehicle.ParkingSpot.IsOccupied = false;

            await _context.SaveChangesAsync();

            return new ParkingTicketViewModel
            {
                LicensePlate = vehicle.LicensePlate,
                EntryTime = vehicle.EntryTime,
                ExitTime = (DateTime)vehicle.ExitTime,
                TimeParked = timeParked.ToString(@"hh\h\ mm\m\ ss\s"),
                TotalAmount = vehicle.TotalAmount,
                ParkingSpotNumber = vehicle.ParkingSpot.Number
            };
        }

        public async Task<IEnumerable<VehicleViewModel>> GetParkedVehiclesAsync()
        {
            return await _context.Vehicles
                .Where(v => v.IsParked)
                .Include(v => v.ParkingSpot)
                .Select(v => new VehicleViewModel
                {
                    Id = v.Id,
                    LicensePlate = v.LicensePlate,
                    Model = v.Model,
                    Color = v.Color,
                    EntryTime = v.EntryTime,
                    ParkingSpotId = v.ParkingSpot.Id,
                    ParkingSpotNumber = v.ParkingSpot.Number
                })
                .ToListAsync();
        }
    }
}