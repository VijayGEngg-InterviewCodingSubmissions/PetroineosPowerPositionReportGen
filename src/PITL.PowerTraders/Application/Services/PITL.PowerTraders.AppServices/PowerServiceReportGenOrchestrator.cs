using Microsoft.Extensions.Logging;
using PITL.PowerTraders.Interfaces;
using PITL.PowerTraders.Models.ResultModels;
using System.Text;

namespace PITL.PowerTraders.AppServices
{
    public class PowerServiceReportGenOrchestrator : IPowerServiceReportGenOrchestrator
    {
        private readonly ILogger<PowerServiceReportGenOrchestrator> _logger;
        private readonly IPowerServiceTradeSubscriberService _powerServiceConsumer;
        private readonly IPowerTradeReportDataService _tradeReportDataService;
        private readonly ICsvReportWriter _csvWriter;

        public PowerServiceReportGenOrchestrator(
            ILogger<PowerServiceReportGenOrchestrator> logger,
            IPowerServiceTradeSubscriberService powerServiceConsumer,
            IPowerTradeReportDataService tradeReportDataService,
            ICsvReportWriter csvWriter)
        {
            _logger = logger;
            _powerServiceConsumer = powerServiceConsumer;
            _tradeReportDataService = tradeReportDataService;
            _csvWriter = csvWriter;
        }

        public async Task<PowerPositionReportFileGenResult> GeneratePowerPositionReport(DateTime date)
        {
            _logger.LogInformation("Generation function of Power Postion Report Started.");

            PowerPositionReportFileGenResult reportGenResult = new PowerPositionReportFileGenResult();

            if(date < DateTime.Today.Date)
            {
                DateTime reportDateTime = new DateTime(date.Year, date.Month, date.Day, 23, 0, 0).AddDays(-1);
                reportGenResult.StartPeriod = reportDateTime;
                _logger.LogInformation($"");

                // Get Trades from PowerService.dll
                var powerTrades = await _powerServiceConsumer.GetTradesAsync(reportDateTime);
                reportGenResult.Trades = powerTrades.ToList();
                _logger.LogInformation($"");

                // Generate Report Data
                var powerVolumeDataGenResult = _tradeReportDataService.GetPowerPeriodVolumeData(powerTrades);
                if (powerVolumeDataGenResult.IsSuccess) reportGenResult.PeriodVolumes = powerVolumeDataGenResult.PowerPeriodVolume;
                _logger.LogInformation($"");

                // Generate CSV File for Report Data
                string reportFileName = GetReportFileName(reportDateTime);
                var reportFileCreationResult = _csvWriter.GeneratePeriodVolumeCsvFile(reportFileName, reportGenResult.PeriodVolumes);
                _logger.LogInformation($"");

                reportGenResult.IsSuccess = powerVolumeDataGenResult.IsSuccess && reportFileCreationResult.IsSuccess;
                reportGenResult.ReportFilePath = reportFileCreationResult.FilePath;
                reportGenResult.CsvReportFileName = reportFileCreationResult.FileName;
            }

            _logger.LogInformation("Generation function of Power Postion Report Finished.");

            return reportGenResult;
        }

        private string GetReportFileName(DateTime reportDateTime)
        {

            return $"PowerPosition_{reportDateTime.AddDays(1).Date.ToString("yyyymmdd")}_{reportDateTime.ToString("HH:mm")}.csv";
        }
    }
}
