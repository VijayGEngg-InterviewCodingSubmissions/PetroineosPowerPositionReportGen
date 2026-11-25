namespace PITL.PowerTraders.Models.Settings
{
    public class PowerReportSetting
    {
        public const string SectionName = "PowerReportSettings";

        public string OutputFolderName { get; set; } = string.Empty;

        public string CsvDelimiter { get; set; } = ",";

        public int ExtractIntervalMinutes { get; set; } = 5;
    }
}
