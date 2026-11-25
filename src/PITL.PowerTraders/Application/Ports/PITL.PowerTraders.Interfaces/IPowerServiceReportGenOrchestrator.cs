using PITL.PowerTraders.Models.ResultModels;

namespace PITL.PowerTraders.Interfaces
{
    public interface IPowerServiceReportGenOrchestrator
    {
        Task<PowerPositionReportFileGenResult> GeneratePowerPositionReport(DateTime date);
    }
}
