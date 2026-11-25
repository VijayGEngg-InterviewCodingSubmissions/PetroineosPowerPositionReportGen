using Microsoft.Extensions.Logging;
using PITL.PowerTraders.IntegrationTests.BaseClasses;
using PITL.PowerTraders.Interfaces;
using PITL.PowerTraders.Models.DomainModels;
using PITL.PowerTraders.PowerServiceConsumer;
using PITL.PowerTraders.PowerServiceConsumer.Mapper;
using Services;

namespace PITL.PowerTraders.IntegrationTests.Adapters
{
    public class PowerServiceTradeSubscriberTests : BaseIntegrationTest
    {
        private IPowerTradeProviderService _sut;

        public PowerServiceTradeSubscriberTests() : base()
        {
            ILogger<PowerTradeProviderService> sutLogger = LoggerFactory.CreateLogger<PowerTradeProviderService>();
            IMapper<PowerTrade, PowerTradeDTO> mapper = new PowerTradeMapper();
            _sut = new PowerTradeProviderService(sutLogger, mapper);
        }

        [Fact]
        public async Task PowerServiceTradeSubscriberTests_ServiceInstanceCreatedProperly()
        {
            Assert.NotNull(_sut);
            Assert.IsType<PowerTradeProviderService>(_sut);
        }

        [Fact]
        public async Task PowerServiceTradeSubscriberTests_CallGetTradesForToday_Positive()
        {
            Assert.NotNull(_sut);
            Assert.IsType<PowerTradeProviderService>(_sut);

            DateTime today = DateTime.Today;
            var powerTrades = _sut.GetTrades(today);

            Assert.NotNull(powerTrades);
            Assert.IsType<List<PowerTradeDTO>>(powerTrades);
        }

        [Fact]
        public async Task PowerServiceTradeSubscriberTests_CallGetTradesAsyncForToday_Positive()
        {
            Assert.NotNull(_sut);
            Assert.IsType<PowerTradeProviderService>(_sut);

            DateTime today = DateTime.Today;
            var powerTrades = await _sut.GetTradesAsync(today);

            Assert.NotNull(powerTrades);
            Assert.IsType<List<PowerTradeDTO>>(powerTrades);
        }
    }
}
