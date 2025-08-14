using ParkingSystem.Web.Models;

namespace ParkingSystem.Tests.Models
{
    public class DashboardViewModelTests
    {
        [Fact]
        public void OccupiedSpots_WithEmptyList_ReturnsZero()
        {
            // Arrange
            var viewModel = new DashboardViewModel();

            // Act
            var result = viewModel.OccupiedSpots;

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void OccupiedSpots_WithMixedSpots_ReturnsCorrectCount()
        {
            // Arrange
            var viewModel = new DashboardViewModel
            {
                ParkingSpots = new List<ParkingSpotDto>
                {
                    new() { Id = 1, Number = "A1", IsOccupied = true },
                    new() { Id = 2, Number = "A2", IsOccupied = false },
                    new() { Id = 3, Number = "A3", IsOccupied = true },
                    new() { Id = 4, Number = "A4", IsOccupied = false }
                }
            };

            // Act
            var result = viewModel.OccupiedSpots;

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public void AvailableSpots_WithEmptyList_ReturnsZero()
        {
            // Arrange
            var viewModel = new DashboardViewModel();

            // Act
            var result = viewModel.AvailableSpots;

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void AvailableSpots_WithMixedSpots_ReturnsCorrectCount()
        {
            // Arrange
            var viewModel = new DashboardViewModel
            {
                ParkingSpots = new List<ParkingSpotDto>
                {
                    new() { Id = 1, Number = "A1", IsOccupied = true },
                    new() { Id = 2, Number = "A2", IsOccupied = false },
                    new() { Id = 3, Number = "A3", IsOccupied = true },
                    new() { Id = 4, Number = "A4", IsOccupied = false }
                }
            };

            // Act
            var result = viewModel.AvailableSpots;

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public void TotalSpots_WithEmptyList_ReturnsZero()
        {
            // Arrange
            var viewModel = new DashboardViewModel();

            // Act
            var result = viewModel.TotalSpots;

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void TotalSpots_WithSpots_ReturnsCorrectCount()
        {
            // Arrange
            var viewModel = new DashboardViewModel
            {
                ParkingSpots = new List<ParkingSpotDto>
                {
                    new() { Id = 1, Number = "A1", IsOccupied = true },
                    new() { Id = 2, Number = "A2", IsOccupied = false },
                    new() { Id = 3, Number = "A3", IsOccupied = true }
                }
            };

            // Act
            var result = viewModel.TotalSpots;

            // Assert
            Assert.Equal(3, result);
        }

        [Fact]
        public void OccupancyRate_WithEmptyList_ReturnsZero()
        {
            // Arrange
            var viewModel = new DashboardViewModel();

            // Act
            var result = viewModel.OccupancyRate;

            // Assert
            Assert.Equal(0.0, result);
        }

        [Fact]
        public void OccupancyRate_WithAllOccupied_ReturnsOne()
        {
            // Arrange
            var viewModel = new DashboardViewModel
            {
                ParkingSpots = new List<ParkingSpotDto>
                {
                    new() { Id = 1, Number = "A1", IsOccupied = true },
                    new() { Id = 2, Number = "A2", IsOccupied = true }
                }
            };

            // Act
            var result = viewModel.OccupancyRate;

            // Assert
            Assert.Equal(1.0, result);
        }

        [Fact]
        public void OccupancyRate_WithHalfOccupied_ReturnsHalf()
        {
            // Arrange
            var viewModel = new DashboardViewModel
            {
                ParkingSpots = new List<ParkingSpotDto>
                {
                    new() { Id = 1, Number = "A1", IsOccupied = true },
                    new() { Id = 2, Number = "A2", IsOccupied = false },
                    new() { Id = 3, Number = "A3", IsOccupied = true },
                    new() { Id = 4, Number = "A4", IsOccupied = false }
                }
            };

            // Act
            var result = viewModel.OccupancyRate;

            // Assert
            Assert.Equal(0.5, result);
        }

        [Fact]
        public void OccupancyRate_WithPartialOccupancy_ReturnsCorrectRate()
        {
            // Arrange
            var viewModel = new DashboardViewModel
            {
                ParkingSpots = new List<ParkingSpotDto>
                {
                    new() { Id = 1, Number = "A1", IsOccupied = true },
                    new() { Id = 2, Number = "A2", IsOccupied = false },
                    new() { Id = 3, Number = "A3", IsOccupied = false }
                }
            };

            // Act
            var result = viewModel.OccupancyRate;

            // Assert
            Assert.Equal(1.0/3.0, result, precision: 10);
        }

        [Fact]
        public void IsLoading_DefaultValue_IsFalse()
        {
            // Arrange & Act
            var viewModel = new DashboardViewModel();

            // Assert
            Assert.False(viewModel.IsLoading);
        }

        [Fact]
        public void IsLoading_CanBeSetToTrue()
        {
            // Arrange
            var viewModel = new DashboardViewModel();

            // Act
            viewModel.IsLoading = true;

            // Assert
            Assert.True(viewModel.IsLoading);
        }

        [Fact]
        public void ErrorMessage_DefaultValue_IsNull()
        {
            // Arrange & Act
            var viewModel = new DashboardViewModel();

            // Assert
            Assert.Null(viewModel.ErrorMessage);
        }

        [Fact]
        public void ErrorMessage_CanBeSet()
        {
            // Arrange
            var viewModel = new DashboardViewModel();
            var errorMessage = "Test error message";

            // Act
            viewModel.ErrorMessage = errorMessage;

            // Assert
            Assert.Equal(errorMessage, viewModel.ErrorMessage);
        }

        [Fact]
        public void ParkingSpots_DefaultValue_IsEmptyList()
        {
            // Arrange & Act
            var viewModel = new DashboardViewModel();

            // Assert
            Assert.NotNull(viewModel.ParkingSpots);
            Assert.Empty(viewModel.ParkingSpots);
        }

        [Fact]
        public void CalculatedProperties_UpdateWhenParkingSpotsChange()
        {
            // Arrange
            var viewModel = new DashboardViewModel();
            
            // Initially empty
            Assert.Equal(0, viewModel.TotalSpots);
            Assert.Equal(0, viewModel.OccupiedSpots);
            Assert.Equal(0, viewModel.AvailableSpots);
            Assert.Equal(0.0, viewModel.OccupancyRate);

            // Act - Add spots
            viewModel.ParkingSpots = new List<ParkingSpotDto>
            {
                new() { Id = 1, Number = "A1", IsOccupied = true },
                new() { Id = 2, Number = "A2", IsOccupied = false }
            };

            // Assert - Properties updated
            Assert.Equal(2, viewModel.TotalSpots);
            Assert.Equal(1, viewModel.OccupiedSpots);
            Assert.Equal(1, viewModel.AvailableSpots);
            Assert.Equal(0.5, viewModel.OccupancyRate);
        }
    }
}