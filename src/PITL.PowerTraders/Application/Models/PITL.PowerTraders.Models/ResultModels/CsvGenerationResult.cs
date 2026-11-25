namespace PITL.PowerTraders.Models.ResultModels
{
    public sealed record CsvGenerationResult : BaseResult
    {
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
    }
}
