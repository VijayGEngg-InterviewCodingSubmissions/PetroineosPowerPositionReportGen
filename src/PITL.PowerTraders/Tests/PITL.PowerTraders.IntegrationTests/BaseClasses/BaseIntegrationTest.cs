using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace PITL.PowerTraders.IntegrationTests.BaseClasses
{
    public abstract class BaseIntegrationTest
    {
        private NullLoggerFactory _loggerFactory;
        private IConfigurationRoot _configuration;
        private IServiceCollection _services;
        private ServiceProvider _serviceProvider;

        public BaseIntegrationTest()
        {
            _loggerFactory = new NullLoggerFactory();
            _services = new ServiceCollection();
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Test.json")
                .Build();
            _services.AddSingleton<IConfiguration>(_configuration);
            _services.AddSingleton<NullLoggerFactory>(_loggerFactory);
            _services.AddLogging(builder => builder.AddConsole());

            _serviceProvider = _services.BuildServiceProvider();
        }

        public IServiceCollection Services => _services;
        public IConfigurationRoot Configuration => _configuration;
        public NullLoggerFactory LoggerFactory => _loggerFactory;
        public ServiceProvider ServiceProvider => _serviceProvider;
    }
}
