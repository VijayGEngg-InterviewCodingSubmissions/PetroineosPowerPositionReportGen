using PITL.PowerTraders.Models.DomainModels;

namespace PITL.PowerTraders.Interfaces
{
    public interface IPowerTradeProviderService
    {
        IEnumerable<PowerTradeDTO> GetTrades(DateTime date);

        Task<IEnumerable<PowerTradeDTO>> GetTradesAsync(DateTime date);
    }
}
