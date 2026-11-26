using Microsoft.Extensions.Logging;
using NSubstitute;
using PITL.PowerTraders.AppServices.Services;
using PITL.PowerTraders.Models.DomainModels;

namespace PITL.PowerTraders.UnitTests.Services;

public class PowerTradeReportDataServiceTests
{
    private readonly PowerTradeReportDataService _sut;

    public PowerTradeReportDataServiceTests()
    {
        var logger =
            Substitute.For<ILogger<PowerTradeReportDataService>>();

        _sut = new PowerTradeReportDataService(logger);
    }

    [Fact]
    public void GetPowerPeriodVolumeData_WhenInputIsNull_ReturnsFailure()
    {
        // Act
        var result =
            _sut.GetPowerPeriodVolumeData(null);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(
            "No Data found for processing",
            result.ErrorMessage);
    }

    [Fact]
    public void GetPowerPeriodVolumeData_WhenInputIsEmpty_ReturnsFailure()
    {
        // Arrange
        var trades = Enumerable.Empty<PowerTradeDTO>();

        // Act
        var result =
            _sut.GetPowerPeriodVolumeData(trades);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(
            "No Data found for processing",
            result.ErrorMessage);
    }

    [Fact]
    public void GetPowerPeriodVolumeData_WithSinglePeriod_ReturnsCorrectVolume()
    {
        // Arrange
        var trades = new List<PowerTradeDTO>
        {
            new()
            {
                Periods =
                [
                    new PowerPeriodDTO
                    {
                        Period = 1,
                        Volume = 100
                    }
                ]
            }
        };

        // Act
        var result =
            _sut.GetPowerPeriodVolumeData(trades);

        // Assert
        Assert.True(result.IsSuccess);

        Assert.Single(result.PowerPeriodVolume);

        Assert.Equal(
            100,
            result.PowerPeriodVolume.First().Volume);
    }

    [Fact]
    public void GetPowerPeriodVolumeData_ShouldSumVolumesAcrossTrades()
    {
        // Arrange
        var trades = new List<PowerTradeDTO>
        {
            new()
            {
                Periods =
                [
                    new PowerPeriodDTO
                    {
                        Period = 1,
                        Volume = 100
                    }
                ]
            },
            new()
            {
                Periods =
                [
                    new PowerPeriodDTO
                    {
                        Period = 1,
                        Volume = 50
                    }
                ]
            }
        };

        // Act
        var result =
            _sut.GetPowerPeriodVolumeData(trades);

        // Assert
        Assert.True(result.IsSuccess);

        var volume =
            result.PowerPeriodVolume
                .Single(x => x.LocalTime ==
                             StaticMappings.PeriodToHourMapping[1]);

        Assert.Equal(150, volume.Volume);
    }

    [Fact]
    public void GetPowerPeriodVolumeData_ShouldHandlePositiveAndNegativeVolumes()
    {
        // Arrange
        var trades = new List<PowerTradeDTO>
        {
            new()
            {
                Periods =
                [
                    new PowerPeriodDTO
                    {
                        Period = 1,
                        Volume = 100
                    }
                ]
            },
            new()
            {
                Periods =
                [
                    new PowerPeriodDTO
                    {
                        Period = 1,
                        Volume = -40
                    }
                ]
            }
        };

        // Act
        var result =
            _sut.GetPowerPeriodVolumeData(trades);

        // Assert
        Assert.True(result.IsSuccess);

        Assert.Equal(
            60,
            result.PowerPeriodVolume.Single().Volume);
    }

    [Fact]
    public void GetPowerPeriodVolumeData_ShouldCreateOneResultPerUniquePeriod()
    {
        // Arrange
        var trades = new List<PowerTradeDTO>
        {
            new()
            {
                Periods =
                [
                    new PowerPeriodDTO
                    {
                        Period = 1,
                        Volume = 100
                    },
                    new PowerPeriodDTO
                    {
                        Period = 2,
                        Volume = 200
                    }
                ]
            }
        };

        // Act
        var result =
            _sut.GetPowerPeriodVolumeData(trades);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.PowerPeriodVolume.Count);
    }
}
