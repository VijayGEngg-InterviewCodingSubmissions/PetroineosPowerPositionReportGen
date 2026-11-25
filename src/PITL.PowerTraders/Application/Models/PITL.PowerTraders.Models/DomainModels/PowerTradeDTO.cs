namespace PITL.PowerTraders.Models.DomainModels
{
    public class PowerTradeDTO
    {
        public DateTime Date { get; set; }

        public List<PowerPeriodDTO> Periods { get; set; }

        public PowerTradeDTO()
        {
            Periods = new List<PowerPeriodDTO>();
        }
    }
}
