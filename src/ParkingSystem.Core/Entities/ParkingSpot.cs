using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ParkingSystem.Core.Enums;

namespace ParkingSystem.Core.Entities
{
    public class ParkingSpot
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(10)]
        public string Number { get; set; } = string.Empty;
        
        public bool IsOccupied { get; set; }
        public SpotType Type { get; set; }
        
        public List<Vehicle> VehicleHistory { get; set; } = new();
        
        public Vehicle? CurrentVehicle => VehicleHistory?.FirstOrDefault(v => v.IsParked);
    }
}