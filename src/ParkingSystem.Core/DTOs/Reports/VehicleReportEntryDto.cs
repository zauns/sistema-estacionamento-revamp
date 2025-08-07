using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingSystem.Core.DTOs.Reports
{
    public class VehicleReportEntryDto
    {
        public string LicensePlate { get; set; } = string.Empty;
        public DateTime EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }
        public decimal TotalAmount { get; set; }
    }
}