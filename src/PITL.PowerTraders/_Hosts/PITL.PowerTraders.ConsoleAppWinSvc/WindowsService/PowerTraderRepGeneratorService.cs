using Microsoft.Extensions.Logging;
using Topshelf;

namespace PITL.PowerTraders.ConsoleAppWinSvc.WindowsService
{
    public class PowerTraderRepGeneratorService : ServiceControl
    {
        private readonly ILogger<PowerTraderRepGeneratorService> _logger;
        //private readonly IWorkerProcessor _processor;

        public PowerTraderRepGeneratorService(ILogger<PowerTraderRepGeneratorService> logger)
            //,IWorkerProcessor workerProcessor) 
        { 
            _logger = logger;
            //_processor = workerProcessor;
        }

        public bool Start(HostControl hostControl)
        {
            _logger.LogInformation("Starting Worker Service");
            //_processor.Start();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _logger.LogInformation("Stopping Worker Service");
            //_processor.Stop();
            return true;
        }
    }
}
