using PITL.PowerTraders.Models.DomainModels;
using PITL.PowerTraders.Models.ResultModels;

namespace PITL.PowerTraders.Interfaces
{
    public interface ICsvReportWriter
    {
        CsvGenerationResult GeneratePeriodVolumeCsvFile(string fileName, List<PeriodVolumeDTO> periodVolumeData);
    }
}
