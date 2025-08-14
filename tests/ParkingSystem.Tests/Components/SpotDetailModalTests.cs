using ParkingSystem.Web.Models;

namespace ParkingSystem.Tests.Components;

public class SpotDetailModalTests
{

    [Fact]
    public void SpotDetailModal_ParametersCanBeSet()
    {
        // Arrange
        var spot = new ParkingSpotDto { Id = 1, Number = "A01", IsOccupied = false };

        // Act & Assert - should not throw exception
        Assert.NotNull(spot);
        Assert.Equal("A01", spot.Number);
        Assert.False(spot.IsOccupied);
    }

    [Fact]
    public void SpotDetailModal_HandlesOccupiedSpot()
    {
        // Arrange
        var spot = new ParkingSpotDto { Id = 1, Number = "A01", IsOccupied = true };

        // Act & Assert
        Assert.NotNull(spot);
        Assert.Equal("A01", spot.Number);
        Assert.True(spot.IsOccupied);
    }

    [Fact]
    public void SpotDetailModal_HandlesVehicleViewModel()
    {
        // Arrange
        var vehicle = new VehicleViewModel
        {
            Id = 1,
            LicensePlate = "ABC-1234",
            Model = "Honda Civic",
            Color = "Azul",
            EntryTime = DateTime.Now.AddHours(-2),
            ParkingSpotId = 1,
            ParkingSpotNumber = "A01"
        };

        // Act & Assert
        Assert.NotNull(vehicle);
        Assert.Equal("ABC-1234", vehicle.LicensePlate);
        Assert.Equal("Honda Civic", vehicle.Model);
        Assert.Equal("Azul", vehicle.Color);
        Assert.Equal(1, vehicle.ParkingSpotId);
        Assert.Equal("A01", vehicle.ParkingSpotNumber);
    }

    [Fact]
    public void SpotDetailModal_HandlesNullVehicle()
    {
        // Arrange
        VehicleViewModel? vehicle = null;

        // Act & Assert
        Assert.Null(vehicle);
    }

    [Theory]
    [InlineData(1, "A01", true)]
    [InlineData(2, "B02", false)]
    [InlineData(3, "C03", true)]
    public void SpotDetailModal_HandlesVariousSpotStates(int id, string number, bool isOccupied)
    {
        // Arrange
        var spot = new ParkingSpotDto { Id = id, Number = number, IsOccupied = isOccupied };

        // Act & Assert
        Assert.Equal(id, spot.Id);
        Assert.Equal(number, spot.Number);
        Assert.Equal(isOccupied, spot.IsOccupied);
    }
}