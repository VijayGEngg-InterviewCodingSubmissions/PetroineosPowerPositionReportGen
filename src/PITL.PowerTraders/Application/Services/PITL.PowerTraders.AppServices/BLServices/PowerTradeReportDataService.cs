using Microsoft.Extensions.Logging;
using PITL.PowerTraders.Interfaces;
using PITL.PowerTraders.Models.DomainModels;
using PITL.PowerTraders.Models.ResultModels;

namespace PITL.PowerTraders.AppServices.Services
{
    public class PowerTradeReportDataService : IPowerTradeReportDataService
    {
        private readonly ILogger<PowerTradeReportDataService> _logger;

        public PowerTradeReportDataService(ILogger<PowerTradeReportDataService> logger) => _logger = logger;

        public PowerPositionDataResult GetPowerPeriodVolumeData(IEnumerable<PowerTradeDTO> datesPowerTrades)
        {
            PowerPositionDataResult result = new PowerPositionDataResult();

            if (datesPowerTrades != null && datesPowerTrades.Any())
            {

            }
            else
            {
                result.ErrorMessage = "No Data found for processing";
            }

            return result;
        }
    }
}
