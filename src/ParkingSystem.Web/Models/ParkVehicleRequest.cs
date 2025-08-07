using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingSystem.Web.Models
{
    public class ParkVehicleRequest
    {
        public string LicensePlate { get; set; } = string.Empty;
        public int SpotId { get; set; }
        public string? Model { get; set; }
        public string? Color { get; set; }
    }
}