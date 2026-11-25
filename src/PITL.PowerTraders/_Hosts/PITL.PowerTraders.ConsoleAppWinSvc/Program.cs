using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PITL.PowerTraders.ConsoleAppWinSvc.WindowsService;
using PITL.PowerTraders.Interfaces;
using PITL.PowerTraders.PowerServiceConsumer;
using Serilog;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)          // Donot use Directory.GetCurrentDirectory() it returns C:\Windows\System32 we don't want that
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        Path.Combine(AppContext.BaseDirectory, "logs", "PowerTraderRepGen-Service-.log"),    // Similar to above
        rollingInterval: RollingInterval.Day
    )
    .CreateLogger();

var services = new ServiceCollection();
services.AddSingleton<IConfiguration>(configuration);
services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddSerilog(dispose: false);
});
services.AddTransient<IPowerServiceTradeSubscriberService, PowerServiceTradeSubscriberService>();
services.AddTransient<PowerTraderRepGeneratorService>();

await using var serviceProvider = services.BuildServiceProvider();

try
{

}
catch(Exception svcExcep)
{
    Log.Error($"Error encountered while Starting the Windows Service. Error Details: {svcExcep.Message}");
}
finally
{
    await serviceProvider.DisposeAsync();
    await Log.CloseAndFlushAsync();
}