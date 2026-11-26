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
                // Flatten the structure first i.e. Make a list of all the Periods
                List<PowerPeriodDTO> allPwrPeriods = datesPowerTrades.SelectMany(pt => pt.Periods).ToList();

                // GroupBy all Periods by Peiod number as their can be multiple records for same period number.
                Dictionary<int, List<PowerPeriodDTO>> grpTrades = allPwrPeriods.GroupBy(p => p.Period)
                                    .ToDictionary(
                                        g => g.Key, 
                                        g => g.ToList());

                // Now we have all the List<PowerPeriodDTO> for the Period
                // We can use Sum function to ADD or SUBTRACT Volumes of a Period
                result.PowerPeriodVolume = new List<PeriodVolumeDTO>();
                grpTrades.Keys.ToList().ForEach(prd =>
                {
                    PeriodVolumeDTO periodVolume = new PeriodVolumeDTO();
                    periodVolume.LocalTime = StaticMappings.PeriodToHourMapping[prd];
                    periodVolume.Volume = grpTrades[prd].Sum(p => p.Volume);

                    result.PowerPeriodVolume.Add(periodVolume);
                });
                result.IsSuccess = true;
            }
            else
            {
                result.ErrorMessage = "No Data found for processing";
            }

            return result;
        }
    }
}
