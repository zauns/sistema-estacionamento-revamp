using Microsoft.EntityFrameworkCore;
using ParkingSystem.Core.DTOs.Reports;
using ParkingSystem.Infrastructure.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingSystem.API.Services
{
    public class ReportService : IReportService
    {
        private readonly ParkingDbContext _context;

        public ReportService(ParkingDbContext context)
        {
            _context = context;
        }

        public async Task<DailyReportDto> GenerateDailyReportAsync(DateOnly date)
        {
            var startDate = date.ToDateTime(TimeOnly.MinValue);
            var endDate = startDate.AddDays(1);

            // Busca todos os veículos que saíram nesse dia.
            // A receita é contada na saída.
            var vehiclesExited = await _context.Vehicles
                .Where(v => v.ExitTime >= startDate && v.ExitTime < endDate)
                .ToListAsync();

            var report = new DailyReportDto
            {
                ReportDate = startDate,
                TotalVehiclesEntered = vehiclesExited.Count,
                TotalRevenue = vehiclesExited.Sum(v => v.TotalAmount),
                VehicleEntries = vehiclesExited.Select(v => new VehicleReportEntryDto
                {
                    LicensePlate = v.LicensePlate,
                    EntryTime = v.EntryTime,
                    ExitTime = v.ExitTime,
                    TotalAmount = v.TotalAmount
                }).ToList()
            };

            return report;
        }
    }
}