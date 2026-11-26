using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using PITL.PowerTraders.AppServices;
using PITL.PowerTraders.AppServices.Services;
using PITL.PowerTraders.CsvGeneratorService;
using PITL.PowerTraders.Interfaces;
using PITL.PowerTraders.Models.DomainModels;
using PITL.PowerTraders.Models.Settings;
using PITL.PowerTraders.PowerServiceConsumer;
using PITL.PowerTraders.PowerServiceConsumer.Mapper;
using Services;

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
            // CSV Report Writer Instancing
            ILogger<CsvReportWriter> mockCSVWriterLogger = Substitute.For<ILogger<CsvReportWriter>>();
            _mockCSVReportWriter = Substitute.For<CsvReportWriter>(mockCSVWriterLogger);

            // Power Trade Provider Instancing
            IMapper<PowerTrade, PowerTradeDTO> mockPwrTrdMapper = Substitute.For<PowerTradeMapper>();
            ILogger<PowerTradeProviderService> mockPowerServiceTradeSubSvcLogger = Substitute.For<ILogger<PowerTradeProviderService>>();
            _mockPowerTradeProviderSvc = Substitute.For<PowerTradeProviderService>(mockPowerServiceTradeSubSvcLogger, mockPwrTrdMapper);

            // Power Trade Report Data Service Instancing
            ILogger<PowerTradeReportDataService> mockPowerTradeReptDataSvcLogger = Substitute.For<ILogger<PowerTradeReportDataService>>();
            _mockPowerTradeReportDataSvc = Substitute.For<PowerTradeReportDataService>(mockPowerTradeReptDataSvcLogger);

            PowerReportSetting powerReportSetting = new PowerReportSetting();
            powerReportSetting.CsvDelimiter = ",";
            powerReportSetting.ExtractIntervalMinutes = 1;
            powerReportSetting.OutputFolderName = "TestOutputDir";
            IOptions<PowerReportSetting> _reportSettingsOpts = Options.Create<PowerReportSetting>(powerReportSetting);

            // SUT Instancing
            ILogger<PowerServiceReportGenOrchestrator> orchServiceLogger = Substitute.For<ILogger<PowerServiceReportGenOrchestrator>>();
            _sut = new PowerServiceReportGenOrchestrator(orchServiceLogger,
                _mockPowerTradeProviderSvc,
                _mockPowerTradeReportDataSvc,
                _mockCSVReportWriter,
                _reportSettingsOpts);
        }

        [Fact]
        public async Task OrchestrationService_EndToEndTest_Positive()
        {
            Assert.NotNull(_sut);
            Assert.IsType<PowerServiceReportGenOrchestrator>(_sut);

            // Pick a date that is 7 days back
            DateTime reptDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day).AddDays(-7);

            var result = await _sut.GeneratePowerPositionReportAsync(reptDate);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            string genFileFullPath = Path.Combine(result.ReportFilePath, result.CsvReportFileName);
            Assert.True(File.Exists(genFileFullPath));

            _mockPowerTradeProviderSvc.Received(1)
                .GetTradesAsync(reptDate);

            _mockPowerTradeReportDataSvc.Received(1)
                .GetPowerPeriodVolumeData(Arg.Any<List<PowerTradeDTO>>());

            _mockCSVReportWriter.Received(1)
                .GeneratePeriodVolumeCsvFile(Arg.Any<string>(), Arg.Any<List<PeriodVolumeDTO>>());
        }
    }
}
