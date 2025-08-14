using ParkingSystem.Web.Components.Shared;
using Xunit;

namespace ParkingSystem.Tests.Components;

public class OccupancyProgressBarTests
{
    [Theory]
    [InlineData(0, 100, 0.0)]
    [InlineData(25, 100, 0.25)]
    [InlineData(50, 100, 0.5)]
    [InlineData(75, 100, 0.75)]
    [InlineData(100, 100, 1.0)]
    public void OccupancyProgressBar_CalculatesCorrectOccupancyRates(int occupied, int total, double expectedRate)
    {
        // Arrange
        var component = new OccupancyProgressBar
        {
            OccupiedSpots = occupied,
            TotalSpots = total
        };

        // Act
        var actualRate = component.OccupancyRate;

        // Assert
        Assert.Equal(expectedRate, actualRate, 2);
    }

    [Fact]
    public void OccupancyProgressBar_HandlesZeroTotalSpots()
    {
        // Arrange
        var component = new OccupancyProgressBar
        {
            OccupiedSpots = 0,
            TotalSpots = 0
        };

        // Act
        var rate = component.OccupancyRate;

        // Assert
        Assert.Equal(0, rate);
    }

    [Fact]
    public void OccupancyProgressBar_HandlesZeroTotalSpotsWithOccupiedSpots()
    {
        // Arrange
        var component = new OccupancyProgressBar
        {
            OccupiedSpots = 5,
            TotalSpots = 0
        };

        // Act
        var rate = component.OccupancyRate;

        // Assert
        Assert.Equal(0, rate);
    }

    [Theory]
    [InlineData(30, 100, 0.3)] // 30% - Should be green (< 50%)
    [InlineData(49, 100, 0.49)] // 49% - Should be green (< 50%)
    [InlineData(50, 100, 0.5)] // 50% - Should be yellow (>= 50% and <= 80%)
    [InlineData(75, 100, 0.75)] // 75% - Should be yellow (>= 50% and <= 80%)
    [InlineData(80, 100, 0.8)] // 80% - Should be yellow (>= 50% and <= 80%)
    [InlineData(81, 100, 0.81)] // 81% - Should be red (> 80%)
    [InlineData(100, 100, 1.0)] // 100% - Should be red (> 80%)
    public void OccupancyProgressBar_CalculatesExpectedOccupancyThresholds(int occupied, int total, double expectedRate)
    {
        // Arrange
        var component = new OccupancyProgressBar
        {
            OccupiedSpots = occupied,
            TotalSpots = total
        };

        // Act
        var actualRate = component.OccupancyRate;

        // Assert
        Assert.Equal(expectedRate, actualRate, 2);

        // Verify the rate falls into expected threshold ranges
        if (expectedRate < 0.5)
        {
            Assert.True(actualRate < 0.5, "Rate should be less than 50% for green threshold");
        }
        else if (expectedRate <= 0.8)
        {
            Assert.True(actualRate >= 0.5 && actualRate <= 0.8, "Rate should be between 50% and 80% for yellow threshold");
        }
        else
        {
            Assert.True(actualRate > 0.8, "Rate should be greater than 80% for red threshold");
        }
    }

    [Fact]
    public void OccupancyProgressBar_PropertiesAreSettable()
    {
        // Arrange
        var component = new OccupancyProgressBar();

        // Act
        component.OccupiedSpots = 42;
        component.TotalSpots = 150;

        // Assert
        Assert.Equal(42, component.OccupiedSpots);
        Assert.Equal(150, component.TotalSpots);
        Assert.Equal(0.28, component.OccupancyRate, 2);
    }
}