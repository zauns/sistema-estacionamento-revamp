using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingSystem.Desktop.Models
{
    public class ParkingSpotDto
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public bool IsOccupied { get; set; }
    }
}