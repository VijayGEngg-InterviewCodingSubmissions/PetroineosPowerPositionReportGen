namespace PITL.PowerTraders.Models.ResultModels
{
    public abstract record BaseResult
    {
        public bool IsSuccess { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
