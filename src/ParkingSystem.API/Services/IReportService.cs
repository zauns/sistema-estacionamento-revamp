using ParkingSystem.Core.DTOs.Reports;
using System;
using System.Threading.Tasks;

namespace ParkingSystem.API.Services
{
    public interface IReportService
    {
        Task<DailyReportDto> GenerateDailyReportAsync(DateOnly date);
    }
}