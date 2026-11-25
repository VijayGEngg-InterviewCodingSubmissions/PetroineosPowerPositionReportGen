using PITL.PowerTraders.AppServices;
using PITL.PowerTraders.AppServices.Services;
using PITL.PowerTraders.CsvGeneratorService;
using PITL.PowerTraders.Interfaces;
using PITL.PowerTraders.Models.DomainModels;
using PITL.PowerTraders.Models.Settings;
using PITL.PowerTraders.PowerServiceConsumer;
using PITL.PowerTraders.PowerServiceConsumer.Mapper;
using PITL.PowerTraders.WinWorkerService;
using Serilog;
using Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddWindowsService(options => 
{
    options.ServiceName = "Power Trader Period Volume Report Generator";
});

// Read configuration
if (builder.Environment.IsProduction())
{
    builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();
}
else if (builder.Environment.IsDevelopment())
{
    builder.Configuration
    .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();
}
else
{
    builder.Configuration
    .AddJsonFile("appsettings.Test.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();
}

builder.Services.Configure<PowerReportSetting>(builder.Configuration.GetSection(PowerReportSetting.SectionName));

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Services.AddSerilog();

// 1. Add Adapter Services
builder.Services.AddScoped<IMapper<PowerPeriod, PowerPeriodDTO>, PowerPeriodMapper>();
builder.Services.AddScoped<IMapper<PowerTrade, PowerTradeDTO>, PowerTradeMapper>();

builder.Services.AddTransient<IPowerTradeProviderService, PowerTradeProviderService>();
builder.Services.AddTransient<ICsvReportWriter, CsvReportWriter>();

// 2. Application Services
builder.Services.AddTransient<IPowerTradeReportDataService, PowerTradeReportDataService>();
builder.Services.AddTransient<IPowerServiceReportGenOrchestrator, PowerServiceReportGenOrchestrator>();

builder.Services.AddHostedService<Worker>();

try
{
    Log.Information("Starting service...");

    var app = builder.Build();

    await app.RunAsync();
}
catch(Exception excep)
{
    Log.Fatal(excep, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
