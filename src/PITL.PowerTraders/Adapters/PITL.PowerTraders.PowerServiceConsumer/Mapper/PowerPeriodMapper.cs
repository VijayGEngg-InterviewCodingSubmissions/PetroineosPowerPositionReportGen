using PITL.PowerTraders.Interfaces;
using PITL.PowerTraders.Models.DomainModels;
using Services;

namespace PITL.PowerTraders.PowerServiceConsumer.Mapper
{
    public class PowerPeriodMapper : IMapper<PowerPeriod, PowerPeriodDTO>
    {
        public IEnumerable<PowerPeriodDTO> MapListToDTOList(IEnumerable<PowerPeriod> sourceList)
        {
            List<PowerPeriodDTO> powerPeriodDTOs = new List<PowerPeriodDTO>();

            if (sourceList != null && sourceList.Any())
                powerPeriodDTOs.AddRange(sourceList.Select(spp => MapToDTO(spp)));

            return powerPeriodDTOs;
        }

        public PowerPeriodDTO MapToDTO(PowerPeriod source)
        {
            return new PowerPeriodDTO
            {
                Period = source.Period,
                Volume = source.Volume,
            };
        }
    }
}
