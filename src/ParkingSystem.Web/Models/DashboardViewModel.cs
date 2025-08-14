namespace ParkingSystem.Web.Models
{
    public class DashboardViewModel
    {
        public List<ParkingSpotDto> ParkingSpots { get; set; } = new();
        
        // Calculated properties
        public int OccupiedSpots => ParkingSpots.Count(s => s.IsOccupied);
        public int AvailableSpots => ParkingSpots.Count(s => !s.IsOccupied);
        public int TotalSpots => ParkingSpots.Count;
        public double OccupancyRate => TotalSpots > 0 ? (double)OccupiedSpots / TotalSpots : 0;
        
        // State properties
        public bool IsLoading { get; set; }
        public string? ErrorMessage { get; set; }
    }
}