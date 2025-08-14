using ParkingSystem.Web.Components.Shared;
using ParkingSystem.Web.Models;
using Xunit;

namespace ParkingSystem.Tests.Components
{
    public class ParkingMapGridTests
    {
        [Fact]
        public void ParkingMapGrid_InitializesWithEmptySpotsList()
        {
            // Arrange & Act
            var component = new ParkingMapGrid();

            // Assert
            Assert.NotNull(component.ParkingSpots);
            Assert.Empty(component.ParkingSpots);
        }

        [Fact]
        public void ParkingMapGrid_ParkingSpotsProperty_CanBeSet()
        {
            // Arrange
            var component = new ParkingMapGrid();
            var spots = new List<ParkingSpotDto>
            {
                new() { Id = 1, Number = "A1", IsOccupied = false },
                new() { Id = 2, Number = "A2", IsOccupied = true }
            };

            // Act
            component.ParkingSpots = spots;

            // Assert
            Assert.Equal(2, component.ParkingSpots.Count);
            Assert.Equal("A1", component.ParkingSpots[0].Number);
            Assert.Equal("A2", component.ParkingSpots[1].Number);
            Assert.False(component.ParkingSpots[0].IsOccupied);
            Assert.True(component.ParkingSpots[1].IsOccupied);
        }

        [Theory]
        [InlineData(true, "ocupada")]
        [InlineData(false, "livre")]
        public void ParkingMapGrid_GetSpotAriaLabel_ReturnsCorrectLabel(bool isOccupied, string expectedStatus)
        {
            // Arrange
            var component = new ParkingMapGrid();
            var spot = new ParkingSpotDto { Id = 1, Number = "A1", IsOccupied = isOccupied };

            // Act
            var result = component.GetSpotAriaLabel(spot);

            // Assert
            Assert.Equal($"Vaga A1 - {expectedStatus}", result);
        }

        [Theory]
        [InlineData(true, MudBlazor.Color.Error)]
        [InlineData(false, MudBlazor.Color.Success)]
        public void ParkingMapGrid_GetSpotColor_ReturnsCorrectColor(bool isOccupied, MudBlazor.Color expectedColor)
        {
            // Arrange
            var component = new ParkingMapGrid();
            var spot = new ParkingSpotDto { Id = 1, Number = "A1", IsOccupied = isOccupied };

            // Act
            var result = component.GetSpotColor(spot);

            // Assert
            Assert.Equal(expectedColor, result);
        }

        [Theory]
        [InlineData(true, "background-color: #f44336 !important; color: white !important;")]
        [InlineData(false, "background-color: #4caf50 !important; color: white !important;")]
        public void ParkingMapGrid_GetSpotStyle_ReturnsCorrectStyle(bool isOccupied, string expectedStyle)
        {
            // Arrange
            var component = new ParkingMapGrid();
            var spot = new ParkingSpotDto { Id = 1, Number = "A1", IsOccupied = isOccupied };

            // Act
            var result = component.GetSpotStyle(spot);

            // Assert
            Assert.Equal(expectedStyle, result);
        }
    }
}