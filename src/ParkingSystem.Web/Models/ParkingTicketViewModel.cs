using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingSystem.Web.Models
{
    public class ParkingTicketViewModel
{
    public required string LicensePlate { get; set; }
    public DateTime EntryTime { get; set; }
    public DateTime ExitTime { get; set; }
    public required string TimeParked { get; set; }
    public decimal TotalAmount { get; set; }
    public required string ParkingSpotNumber { get; set; }
}
}