using PITL.PowerTraders.Models.DomainModels;

namespace PITL.PowerTraders.Models.ResultModels
{
    public sealed record PowerPositionDataResult : BaseResult
    {
        public List<PeriodVolumeDTO> PowerPeriodVolume {  get; set; } = new List<PeriodVolumeDTO>();
    }
}
