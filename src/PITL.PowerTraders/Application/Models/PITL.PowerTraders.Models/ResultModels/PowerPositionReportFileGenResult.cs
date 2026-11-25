using PITL.PowerTraders.Models.DomainModels;

namespace PITL.PowerTraders.Models.ResultModels
{
    public sealed record PowerPositionReportFileGenResult : BaseResult
    {
        public DateTime StartPeriod { get; set; } = default(DateTime);
        public List<PowerTradeDTO> Trades { get; set; } = new List<PowerTradeDTO>();
        public List<PeriodVolumeDTO> PeriodVolumes { get; set; } = new List<PeriodVolumeDTO>();
        public string ReportFilePath { get; set; } = string.Empty;
        public string CsvReportFileName { get; set; } = string.Empty;
    }
}
