using PITL.PowerTraders.Models.DomainModels;
using PITL.PowerTraders.Models.ResultModels;

namespace PITL.PowerTraders.Interfaces
{
    public interface IPowerTradeReportDataService
    {
        PowerPositionDataResult GetPowerPeriodVolumeData(IEnumerable<PowerTradeDTO> datesPowerTrades);
    }
}
