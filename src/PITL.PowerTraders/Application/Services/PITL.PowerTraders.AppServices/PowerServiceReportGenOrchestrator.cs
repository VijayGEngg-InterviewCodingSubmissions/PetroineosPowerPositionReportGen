using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PITL.PowerTraders.Interfaces;
using PITL.PowerTraders.Models.ResultModels;
using PITL.PowerTraders.Models.Settings;

namespace PITL.PowerTraders.AppServices
{
    public class PowerServiceReportGenOrchestrator : IPowerServiceReportGenOrchestrator
    {
        private readonly ILogger<PowerServiceReportGenOrchestrator> _logger;
        private readonly IPowerTradeProviderService _powerServiceConsumer;
        private readonly IPowerTradeReportDataService _tradeReportDataService;
        private readonly ICsvReportWriter _csvWriter;

        private readonly PowerReportSetting _reportSettings;

        public PowerServiceReportGenOrchestrator(
            ILogger<PowerServiceReportGenOrchestrator> logger,
            IPowerTradeProviderService powerServiceConsumer,
            IPowerTradeReportDataService tradeReportDataService,
            ICsvReportWriter csvWriter,
            IOptions<PowerReportSetting> reportSettingOpts)
        {
            _logger = logger;
            _powerServiceConsumer = powerServiceConsumer;
            _tradeReportDataService = tradeReportDataService;
            _csvWriter = csvWriter;
            _reportSettings = reportSettingOpts.Value;
        }

        public async Task<PowerPositionReportFileGenResult> GeneratePowerPositionReportAsync(DateTime date)
        {
            _logger.LogInformation("Generation function of Power Postion Report Started.");

            PowerPositionReportFileGenResult reportGenResult = new PowerPositionReportFileGenResult();

            if (date != DateTime.MinValue && date != DateTime.MaxValue)
            {
                DateTime reportDateTime = new DateTime(date.Year, date.Month, date.Day, 23, 0, 0).AddDays(-1);
                reportGenResult.StartPeriod = reportDateTime;
                _logger.LogInformation($"Created the date and time from where the Periods should start. " +
                    $"Provided DateTime ({date.ToString("yyyy/mm/dd hh:mm")}) to Report Start DateTime ({reportDateTime.ToString("yyyy/mm/dd hh:mm")})");

                // Get Trades from PowerService.dll
                var powerTrades = await _powerServiceConsumer.GetTradesAsync(reportDateTime);
                reportGenResult.Trades = powerTrades.ToList();
                _logger.LogInformation($"With Report Start DateTime: {reportDateTime.ToString("yyyy/mm/dd hh:mm")} received Power Trades list with count: {reportGenResult.Trades.Count}");

                // Generate Report Data
                var powerVolumeDataGenResult = _tradeReportDataService.GetPowerPeriodVolumeData(powerTrades);
                if (powerVolumeDataGenResult.IsSuccess)
                {
                    _logger.LogInformation($"To generate the data for report, report data service was called. " +
                        $"It successfully generated Power Period records with count: {powerVolumeDataGenResult.PowerPeriodVolume.Count}");
                    reportGenResult.PeriodVolumes = powerVolumeDataGenResult.PowerPeriodVolume;
                }

                // Generate CSV File for Report Data if Power Volume report data is generated
                CsvGenerationResult reportFileCreationResult = null;
                if (powerVolumeDataGenResult.IsSuccess)
                {
                    string reportFileName = GetReportFileName(reportDateTime);
                    reportGenResult.CsvReportFileName = reportFileName;
                    _logger.LogInformation($"Initiating the CSV Report generating service to generate the Report file with name: {reportFileName}.");

                    reportFileCreationResult = _csvWriter.GeneratePeriodVolumeCsvFile(reportFileName, reportGenResult.PeriodVolumes);
                    reportGenResult.ReportFilePath = reportFileCreationResult.FilePath;
                    
                    _logger.LogInformation($"Finished generating the CSV Report file: {Path.Combine(reportGenResult.ReportFilePath, reportGenResult.CsvReportFileName)}");
                }

                reportGenResult.IsSuccess = powerVolumeDataGenResult.IsSuccess && reportFileCreationResult.IsSuccess;
            }

            _logger.LogInformation("Generation function of Power Postion Report Finished.");

            return reportGenResult;
        }

        public string GetReportFileName(DateTime reportDateTime)
        {
            if (reportDateTime != DateTime.MinValue && 
                reportDateTime != DateTime.MaxValue)
                return $"PowerPosition_{reportDateTime.AddDays(1).Date.ToString("yyyymmdd")}_{reportDateTime.ToString("HH:mm")}.csv";
            else
                return string.Empty;
        }
    }
}
