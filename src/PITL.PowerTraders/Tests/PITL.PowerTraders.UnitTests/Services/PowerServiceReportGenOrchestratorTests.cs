using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using PITL.PowerTraders.AppServices;
using PITL.PowerTraders.Interfaces;
using PITL.PowerTraders.Models.DomainModels;
using PITL.PowerTraders.Models.ResultModels;
using PITL.PowerTraders.Models.Settings;

namespace PITL.PowerTraders.UnitTests.Services
{
    public class PowerServiceReportGenOrchestratorTests
    {
        private PowerServiceReportGenOrchestrator _sut;

        private IPowerTradeProviderService _mockPowerTradeProviderSvc;
        private IPowerTradeReportDataService _mockPowerTradeReportDataSvc;
        private ICsvReportWriter _mockCSVReportWriter;

        public PowerServiceReportGenOrchestratorTests()
        {
            _mockPowerTradeProviderSvc =
                Substitute.For<IPowerTradeProviderService>();

            _mockPowerTradeReportDataSvc =
                Substitute.For<IPowerTradeReportDataService>();

            _mockCSVReportWriter =
                Substitute.For<ICsvReportWriter>();

            var logger =
                Substitute.For<ILogger<PowerServiceReportGenOrchestrator>>();

            var options = Options.Create(
                new PowerReportSetting
                {
                    CsvDelimiter = ",",
                    OutputFolderName = "TestOutputDir",
                    ExtractIntervalMinutes = 1
                });

            _sut = new PowerServiceReportGenOrchestrator(
                logger,
                _mockPowerTradeProviderSvc,
                _mockPowerTradeReportDataSvc,
                _mockCSVReportWriter,
                options);
        }

        [Fact]
        public async Task OrchestrationService_EndToEndTest_Positive()
        {
            // Arrange
            Assert.NotNull(_sut);

            DateTime reptDate = DateTime.Today.AddDays(-7);

            DateTime expectedReportDate =
                new DateTime(
                    reptDate.Year,
                    reptDate.Month,
                    reptDate.Day,
                    23,
                    0,
                    0)
                .AddDays(-1);

            List<PowerTradeDTO> powerTradeDTOs =
            [
                new PowerTradeDTO
        {
            Date = expectedReportDate
        }
            ];

            List<PeriodVolumeDTO> periodVolumeDTOs =
            [
                new PeriodVolumeDTO
        {
            LocalTime = new TimeOnly(23, 0),
            Volume = 100
        },
        new PeriodVolumeDTO
        {
            LocalTime = new TimeOnly(0, 0),
            Volume = 100
        },
        new PeriodVolumeDTO
        {
            LocalTime = new TimeOnly(1, 0),
            Volume = 100
        }
            ];

            PowerPositionDataResult dataResult =
                new()
                {
                    IsSuccess = true,
                    PowerPeriodVolume = periodVolumeDTOs
                };

            CsvGenerationResult csvResult =
                new()
                {
                    IsSuccess = true,
                    FilePath = @"C:\Temp"
                };

            _mockPowerTradeProviderSvc
                .GetTradesAsync(expectedReportDate)
                .Returns(powerTradeDTOs);

            _mockPowerTradeReportDataSvc
                .GetPowerPeriodVolumeData(
                    Arg.Any<IEnumerable<PowerTradeDTO>>())
                .Returns(dataResult);

            _mockCSVReportWriter
                .GeneratePeriodVolumeCsvFile(
                    Arg.Any<string>(),
                    Arg.Any<List<PeriodVolumeDTO>>())
                .Returns(csvResult);

            // Act
            var result =
                await _sut.GeneratePowerPositionReportAsync(reptDate);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

            await _mockPowerTradeProviderSvc
                .Received(1)
                .GetTradesAsync(expectedReportDate);

            _mockPowerTradeReportDataSvc
                .Received(1)
                .GetPowerPeriodVolumeData(
                    Arg.Any<IEnumerable<PowerTradeDTO>>());

            _mockCSVReportWriter
                .Received(1)
                .GeneratePeriodVolumeCsvFile(
                    Arg.Any<string>(),
                    Arg.Any<List<PeriodVolumeDTO>>());

            Assert.Equal(csvResult.FilePath, result.ReportFilePath);
            Assert.Equal(periodVolumeDTOs.Count, result.PeriodVolumes.Count);
        }

        [Fact]
        public async Task GeneratePowerPositionReportAsync_WhenDateIsMinValue_ReturnsUnsuccessfulResult()
        {
            // Arrange
            var date = DateTime.MinValue;

            // Act
            var result = await _sut.GeneratePowerPositionReportAsync(date);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);

            await _mockPowerTradeProviderSvc
                .DidNotReceive()
                .GetTradesAsync(Arg.Any<DateTime>());

            _mockPowerTradeReportDataSvc
                .DidNotReceive()
                .GetPowerPeriodVolumeData(Arg.Any<IEnumerable<PowerTradeDTO>>());

            _mockCSVReportWriter
                .DidNotReceive()
                .GeneratePeriodVolumeCsvFile(
                    Arg.Any<string>(),
                    Arg.Any<List<PeriodVolumeDTO>>());
        }

        [Fact]
        public async Task GeneratePowerPositionReportAsync_WhenDateIsMaxValue_ReturnsUnsuccessfulResult()
        {
            // Arrange
            var date = DateTime.MaxValue;

            // Act
            var result = await _sut.GeneratePowerPositionReportAsync(date);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);

            await _mockPowerTradeProviderSvc
                .DidNotReceive()
                .GetTradesAsync(Arg.Any<DateTime>());
        }

        [Fact]
        public async Task GeneratePowerPositionReportAsync_ShouldCallProviderWithCalculatedReportDate()
        {
            // Arrange
            var inputDate = new DateTime(2025, 01, 08);

            var expectedReportDate =
                new DateTime(2025, 01, 08, 23, 0, 0)
                    .AddDays(-1);

            _mockPowerTradeProviderSvc
                .GetTradesAsync(expectedReportDate)
                .Returns(new List<PowerTradeDTO>());

            _mockPowerTradeReportDataSvc
                .GetPowerPeriodVolumeData(Arg.Any<IEnumerable<PowerTradeDTO>>())
                .Returns(new PowerPositionDataResult
                {
                    IsSuccess = true,
                    PowerPeriodVolume = new List<PeriodVolumeDTO>()
                });

            _mockCSVReportWriter
                .GeneratePeriodVolumeCsvFile(
                    Arg.Any<string>(),
                    Arg.Any<List<PeriodVolumeDTO>>())
                .Returns(new CsvGenerationResult
                {
                    IsSuccess = true,
                    FilePath = @"C:\Temp"
                });

            // Act
            await _sut.GeneratePowerPositionReportAsync(inputDate);

            // Assert
            await _mockPowerTradeProviderSvc
                .Received(1)
                .GetTradesAsync(expectedReportDate);
        }

        [Fact]
        public async Task GeneratePowerPositionReportAsync_ShouldPopulateTradesIntoResult()
        {
            var trades = new List<PowerTradeDTO>
            {
                new()
                {
                    Date = DateTime.Today
                }
            };

            _mockPowerTradeProviderSvc
                .GetTradesAsync(Arg.Any<DateTime>())
                .Returns(trades);

            _mockPowerTradeReportDataSvc
                .GetPowerPeriodVolumeData(trades)
                .Returns(new PowerPositionDataResult
                {
                    IsSuccess = true,
                    PowerPeriodVolume = new List<PeriodVolumeDTO>()
                });

            _mockCSVReportWriter
                .GeneratePeriodVolumeCsvFile(
                    Arg.Any<string>(),
                    Arg.Any<List<PeriodVolumeDTO>>())
                .Returns(new CsvGenerationResult
                {
                    IsSuccess = true,
                    FilePath = @"C:\Reports"
                });

            var result = await _sut.GeneratePowerPositionReportAsync(DateTime.Today);

            Assert.Single(result.Trades);
        }

        [Fact]
        public async Task GeneratePowerPositionReportAsync_WhenDataGenerationFails_ShouldNotGenerateCsv()
        {
            // Arrange
            _mockPowerTradeProviderSvc
                .GetTradesAsync(Arg.Any<DateTime>())
                .Returns(new List<PowerTradeDTO>());

            _mockPowerTradeReportDataSvc
                .GetPowerPeriodVolumeData(Arg.Any<IEnumerable<PowerTradeDTO>>())
                .Returns(new PowerPositionDataResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Failed"
                });

            // Act
            var result = await _sut.GeneratePowerPositionReportAsync(DateTime.Today);

            // Assert
            Assert.False(result.IsSuccess);

            _mockCSVReportWriter
                .DidNotReceive()
                .GeneratePeriodVolumeCsvFile(
                    Arg.Any<string>(),
                    Arg.Any<List<PeriodVolumeDTO>>());
        }
    }
}
