using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PITL.PowerTraders.AppServices;
using PITL.PowerTraders.AppServices.Services;
using PITL.PowerTraders.CsvGeneratorService;
using PITL.PowerTraders.IntegrationTests.BaseClasses;
using PITL.PowerTraders.Interfaces;
using PITL.PowerTraders.Models.DomainModels;
using PITL.PowerTraders.Models.Settings;
using PITL.PowerTraders.PowerServiceConsumer;
using PITL.PowerTraders.PowerServiceConsumer.Mapper;
using Services;

namespace PITL.PowerTraders.IntegrationTests.Services
{
    public class PowerServiceReportGenOrchestratorTests : BaseIntegrationTest
    {
        private PowerServiceReportGenOrchestrator _sut;

        private IPowerTradeProviderService _powerServiceTradeSubscriberSvc;
        private IPowerTradeReportDataService _powerTradeReportDataSvc;
        private ICsvReportWriter _csvReportWriter;

        public PowerServiceReportGenOrchestratorTests() : base() 
        {
            // CSV Report Writer Instancing
            ILogger<CsvReportWriter> csvWriterLogger = LoggerFactory.CreateLogger<CsvReportWriter>();
            _csvReportWriter = new CsvReportWriter(csvWriterLogger);

            // Power Trade Provider Instancing
            IMapper<PowerTrade, PowerTradeDTO> pwrTrdMapper = new PowerTradeMapper();
            ILogger<PowerTradeProviderService> powerServiceTradeSubSvcLogger = LoggerFactory.CreateLogger<PowerTradeProviderService>();
            _powerServiceTradeSubscriberSvc = new PowerTradeProviderService(powerServiceTradeSubSvcLogger, pwrTrdMapper);

            // Power Trade Report Data Service Instancing
            ILogger<PowerTradeReportDataService> powerTradeReptDataSvcLogger = LoggerFactory.CreateLogger<PowerTradeReportDataService>();
            _powerTradeReportDataSvc = new PowerTradeReportDataService(powerTradeReptDataSvcLogger);

            PowerReportSetting powerReportSetting = new PowerReportSetting();
            powerReportSetting.CsvDelimiter = ",";
            powerReportSetting.ExtractIntervalMinutes = 1;
            powerReportSetting.OutputFolderName = "TestOutputDir";
            IOptions<PowerReportSetting> _reportSettingsOpts = Options.Create<PowerReportSetting>(powerReportSetting);

            // SUT Instancing
            ILogger<PowerServiceReportGenOrchestrator> orchServiceLogger = LoggerFactory.CreateLogger<PowerServiceReportGenOrchestrator>();
            _sut = new PowerServiceReportGenOrchestrator(orchServiceLogger, 
                _powerServiceTradeSubscriberSvc,
                _powerTradeReportDataSvc,
                _csvReportWriter,
                _reportSettingsOpts);
        }

        [Fact]
        public async Task OrchestrationService_EndToEndTest()
        {
            Assert.NotNull(_sut);
            Assert.IsType<PowerServiceReportGenOrchestrator>(_sut);

            // Pick a date roughly a week ago
            DateTime reptDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day).AddDays(-7);

            var result = await _sut.GeneratePowerPositionReportAsync(reptDate);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.True(!string.IsNullOrWhiteSpace(result.CsvReportFileName));
            Assert.True(!string.IsNullOrWhiteSpace(result.ReportFilePath));
            Assert.True(Directory.Exists(result.ReportFilePath));
            Assert.True(result.CsvReportFileName.StartsWith("PowerPosition_"));
            Assert.True(result.CsvReportFileName.EndsWith(".csv"));
            string genFileFullPath = Path.Combine(result.ReportFilePath, result.CsvReportFileName);
            Assert.True(File.Exists(genFileFullPath));
        }
    }
}
