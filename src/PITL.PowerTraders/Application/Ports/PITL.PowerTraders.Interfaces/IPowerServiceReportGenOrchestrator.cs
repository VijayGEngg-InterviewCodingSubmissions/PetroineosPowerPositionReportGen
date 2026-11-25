using PITL.PowerTraders.Models.ResultModels;

namespace PITL.PowerTraders.Interfaces
{
    public interface IPowerServiceReportGenOrchestrator
    {
        Task<PowerPositionReportFileGenResult> GeneratePowerPositionReportAsync(DateTime date);
    }
}
