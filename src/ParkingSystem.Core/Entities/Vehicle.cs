using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ParkingSystem.Core.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(10)]
        public string LicensePlate { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string Model { get; set; } = string.Empty;
        
        [StringLength(30)]
        public string Color { get; set; } = string.Empty;
        
        public DateTime EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }
        public int ParkingSpotId { get; set; }
        
        public ParkingSpot ParkingSpot { get; set; } = null!;
        
        public decimal TotalAmount { get; set; }
        public bool IsParked { get; set; }
    }
}