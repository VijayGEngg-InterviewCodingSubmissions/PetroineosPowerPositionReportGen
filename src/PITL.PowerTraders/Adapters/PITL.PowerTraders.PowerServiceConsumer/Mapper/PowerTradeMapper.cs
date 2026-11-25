using PITL.PowerTraders.Interfaces;
using PITL.PowerTraders.Models.DomainModels;
using Services;

namespace PITL.PowerTraders.PowerServiceConsumer.Mapper
{
    public class PowerTradeMapper : IMapper<PowerTrade, PowerTradeDTO>
    {
        private PowerPeriodMapper _powerPeriodMapper;

        public PowerTradeMapper()
        {
            _powerPeriodMapper = new PowerPeriodMapper();
        }

        public IEnumerable<PowerTradeDTO> MapListToDTOList(IEnumerable<PowerTrade> sourceList)
        {
            List<PowerTradeDTO> powerTradeDTOs = new List<PowerTradeDTO>();

            if (sourceList != null & sourceList.Any())
                powerTradeDTOs.AddRange(sourceList.Select(spt => MapToDTO(spt)));

            return powerTradeDTOs;
        }

        public PowerTradeDTO MapToDTO(PowerTrade source)
        {
            return new PowerTradeDTO
            {
                Date = source.Date,
                Periods = _powerPeriodMapper.MapListToDTOList(source.Periods).ToList(),
            };
        }
    }
}
