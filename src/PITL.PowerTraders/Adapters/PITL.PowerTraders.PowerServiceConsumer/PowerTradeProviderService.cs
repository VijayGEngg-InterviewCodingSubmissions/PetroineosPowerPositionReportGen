using Microsoft.Extensions.Logging;
using PITL.PowerTraders.Models.DomainModels;
using Services;
using PITL.PowerTraders.Interfaces;

namespace PITL.PowerTraders.PowerServiceConsumer
{
    public class PowerTradeProviderService : IPowerTradeProviderService
    {
        private readonly ILogger<PowerTradeProviderService> _logger;
        private readonly IMapper<PowerTrade, PowerTradeDTO> _mapper;

        public PowerTradeProviderService(
            ILogger<PowerTradeProviderService> logger,
            IMapper<PowerTrade, PowerTradeDTO> mapper) 
        {
            _logger = logger;
            _mapper = mapper;
        }

        public IEnumerable<PowerTradeDTO> GetTrades(DateTime date)
        {
            List<PowerTradeDTO> powerTradesForDate = new List<PowerTradeDTO>();

            PowerService powerService = new PowerService();
            List<PowerTrade> datePowerTrades = powerService.GetTrades(date).ToList();
            powerTradesForDate = _mapper.MapListToDTOList(datePowerTrades).ToList();
           
            return powerTradesForDate;
        }

        public async Task<IEnumerable<PowerTradeDTO>> GetTradesAsync(DateTime date)
        {
            List<PowerTradeDTO> powerTradesForDate = new List<PowerTradeDTO>();
            
            PowerService powerService = new PowerService();
            var datePowerTrades = await powerService.GetTradesAsync(date);
            powerTradesForDate = _mapper.MapListToDTOList(datePowerTrades).ToList();

            return powerTradesForDate;
        }
    }
}
