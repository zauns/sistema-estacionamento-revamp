using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingSystem.Core.DTOs.Reports
{
    public class DailyReportDto
    {
        public DateTime ReportDate { get; set; }
        public int TotalVehiclesEntered { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<VehicleReportEntryDto> VehicleEntries { get; set; } = new();
    }
}