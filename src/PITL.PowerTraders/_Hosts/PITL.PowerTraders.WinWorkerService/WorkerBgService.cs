using Microsoft.Extensions.Options;
using PITL.PowerTraders.Interfaces;
using PITL.PowerTraders.Models.ResultModels;
using PITL.PowerTraders.Models.Settings;
using System.Reflection;

namespace PITL.PowerTraders.WinWorkerService
{
    public class WorkerBgService : BackgroundService
    {
        private readonly ILogger<WorkerBgService> _logger;
        private readonly IPowerServiceReportGenOrchestrator _reportGenOrchestrator;
        private readonly PowerReportSetting _reportSettings;

        public WorkerBgService(
            ILogger<WorkerBgService> logger, 
            IPowerServiceReportGenOrchestrator reportGenOrchestrator,
            IOptions<PowerReportSetting> reportSettingsOpts)
        {
            _logger = logger;
            _reportGenOrchestrator = reportGenOrchestrator;
            _reportSettings = reportSettingsOpts.Value;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start called first time after restarting or first start of the back ground service.");

            ExecuteAsync(cancellationToken);

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stop function called.");

            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting the Power Period Report Generator for the first time after starting service.");

            // When the 1st Start is called the Report is generated immediately.
            // And then after the execution is controlled by timer.
            PowerPositionReportFileGenResult result = await _reportGenOrchestrator.GeneratePowerPositionReportAsync(DateTime.UtcNow);
            string outputFolderFileLoc = CopyGeneratedReportToOutputFolder(result);

            if(result.IsSuccess)
            {
                _logger.LogInformation("The Power Report generation finished successfully. " +
                    $"The new report file with name: {result.CsvReportFileName} is created in output folder.");
            }
            else
            {
                _logger.LogInformation("The Power Report generation finished with failure. " +
                    $"Reason for failure: {result.ErrorMessage}.");
            }

            _logger.LogInformation("Starting the polling timer to start Power Period Report Generator. " +
                $"Timer time interval is {_reportSettings.ExtractIntervalMinutes} minutes.");

            using var timer = new PeriodicTimer(TimeSpan.FromMinutes(_reportSettings.ExtractIntervalMinutes));

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                _logger.LogInformation($"Time Interval Elapsed: Worker Started for Power Period Report Generator Time: {DateTimeOffset.Now} ...");

                try
                {
                    PowerPositionReportFileGenResult teResult = await _reportGenOrchestrator.GeneratePowerPositionReportAsync(DateTime.UtcNow);

                    string outFolderFileLoc = CopyGeneratedReportToOutputFolder(result);
                    if (result.IsSuccess)
                        _logger.LogInformation("The Power Report generation finished successfully. " +
                            $"The new report file with name: {result.CsvReportFileName} is created in output folder.");
                    else
                        _logger.LogInformation("The Power Report generation finished with failure. " +
                            $"Reason for failure: {result.ErrorMessage}.");
                }
                catch (Exception excep)
                {
                    _logger.LogError(excep, "Processing failed.");
                }

                _logger.LogInformation($"Power Period Report Generator Worker will next be started after {_reportSettings.ExtractIntervalMinutes} minutes.");
            }
        }

        private string CopyGeneratedReportToOutputFolder(PowerPositionReportFileGenResult result)
        {
            string reportFileFullPath = string.Empty;

            string reportOutputFolder = Path.Combine(Environment.CurrentDirectory, _reportSettings.OutputFolderName);            
            if (!Directory.Exists(reportOutputFolder)) Directory.CreateDirectory(reportOutputFolder);

            if (result != null && result.IsSuccess)
            {
                string destinationFileName = result.CsvReportFileName;
                string destinationFullFilePath = Path.Combine(reportOutputFolder, destinationFileName);
                string sourceFullFilePath = Path.Combine(result.ReportFilePath, result.CsvReportFileName);

                if (!File.Exists(destinationFullFilePath)) File.Delete(destinationFullFilePath);

                File.Move(sourceFullFilePath, destinationFullFilePath);

                reportFileFullPath = destinationFullFilePath;
            }

            return reportFileFullPath;
        }


    }
}
