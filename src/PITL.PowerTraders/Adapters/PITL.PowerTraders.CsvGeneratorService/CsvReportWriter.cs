using CsvHelper;
using Microsoft.Extensions.Logging;
using PITL.PowerTraders.Interfaces;
using PITL.PowerTraders.Models.DomainModels;
using PITL.PowerTraders.Models.ResultModels;

namespace PITL.PowerTraders.CsvGeneratorService
{
    public class CsvReportWriter : ICsvReportWriter
    {
        private readonly ILogger<CsvReportWriter> _logger;

        public CsvReportWriter(ILogger<CsvReportWriter> logger) => _logger = logger;

        public CsvGenerationResult GeneratePeriodVolumeCsvFile(string fileName, List<PeriodVolumeDTO> periodVolumeData)
        {
            CsvGenerationResult csvFileGenResult = new CsvGenerationResult();
            
            _logger.LogInformation($"Starting to write CSV report file ({fileName})");

            if (periodVolumeData != null && periodVolumeData.Any() && !string.IsNullOrWhiteSpace(fileName))
            {
                string tempDirPath = Path.Combine(Environment.CurrentDirectory, "TempGenDir");
                
                if (!Directory.Exists(tempDirPath)) Directory.CreateDirectory(tempDirPath);
                csvFileGenResult.FilePath = tempDirPath;
                csvFileGenResult.FileName = fileName;

                string fullFilePath = Path.Combine(tempDirPath, fileName);

                if (File.Exists(fullFilePath)) File.Delete(fullFilePath);
                
                CsvWriter csvWrt = null;
                try
                {
                    _logger.LogInformation($"Starting to write records({periodVolumeData.Count}) in report file.");

                    csvWrt = CsvWriter.ToFile(fullFilePath, fileOptions: FileOptions.WriteThrough);
                    csvWrt.WriteHeader("Local Time", "Volume");
                    foreach (var periodVolume in periodVolumeData)
                    {
                        csvWrt.WriteField(periodVolume.LocalTime.ToString("HH:mm"));
                        csvWrt.WriteField(periodVolume.Volume);
                        csvWrt.EndRecord();
                    }
                    csvWrt.Flush();

                    csvFileGenResult.IsSuccess = true;

                    _logger.LogInformation($"Finished writing {periodVolumeData.Count} records in report file.");
                }
                catch(Exception excep)
                {
                    csvFileGenResult.ErrorMessage = excep.Message;
                }
                finally
                {
                    csvWrt?.Dispose();
                    csvWrt = null;
                }
            }

            _logger.LogInformation($"Finished writing CSV report file ({fileName}).");

            return csvFileGenResult;
        }
    }
}
