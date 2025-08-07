using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingSystem.Web.Models
{
    public class VehicleViewModel
{
    public int Id { get; set; }
    public required string LicensePlate { get; set; }
    public required string Model { get; set; }
    public required string Color { get; set; }
    public DateTime EntryTime { get; set; }
    public int ParkingSpotId { get; set; }
    public required string ParkingSpotNumber { get; set; }
}
}