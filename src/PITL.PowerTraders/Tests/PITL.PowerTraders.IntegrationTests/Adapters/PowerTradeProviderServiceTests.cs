using Microsoft.Extensions.Logging;
using PITL.PowerTraders.IntegrationTests.BaseClasses;
using PITL.PowerTraders.Interfaces;
using PITL.PowerTraders.Models.DomainModels;
using PITL.PowerTraders.PowerServiceConsumer;
using PITL.PowerTraders.PowerServiceConsumer.Mapper;
using Services;

namespace PITL.PowerTraders.IntegrationTests.Adapters
{
    public class PowerTradeProviderServiceTests : BaseIntegrationTest
    {
        private IPowerTradeProviderService _sut;

        public PowerTradeProviderServiceTests() : base()
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
        public async Task PowerServiceTradeSubscriberTests_CallGetTrades_ForToday_Positive()
        {
            Assert.NotNull(_sut);
            Assert.IsType<PowerTradeProviderService>(_sut);

            DateTime today = DateTime.Today;
            var powerTrades = _sut.GetTrades(today);

            Assert.NotNull(powerTrades);
            Assert.IsType<List<PowerTradeDTO>>(powerTrades);
        }

        [Fact]
        public async Task PowerServiceTradeSubscriberTests_CallGetTrades_For_DateTimeMaxValue_GivesData()
        {
            Assert.NotNull(_sut);
            Assert.IsType<PowerTradeProviderService>(_sut);

            DateTime date = DateTime.MaxValue;
            var powerTrades = _sut.GetTrades(date);

            Assert.NotNull(powerTrades);
            Assert.IsType<List<PowerTradeDTO>>(powerTrades);
        }

        [Fact]
        public async Task PowerServiceTradeSubscriberTests_CallGetTrades_For_DateTimeMinValue_ThrowsOutOfRangeError()
        {
            Assert.NotNull(_sut);
            Assert.IsType<PowerTradeProviderService>(_sut);

            DateTime date = DateTime.MinValue;
            Assert.Throws<ArgumentOutOfRangeException>(() => _sut.GetTrades(date));
        }

        [Fact]
        public async Task PowerServiceTradeSubscriberTests_CallGetTradesAsync_ForToday_Positive()
        {
            Assert.NotNull(_sut);
            Assert.IsType<PowerTradeProviderService>(_sut);

            DateTime today = DateTime.Today;
            var powerTrades = await _sut.GetTradesAsync(today);

            Assert.NotNull(powerTrades);
            Assert.IsType<List<PowerTradeDTO>>(powerTrades);
        }

        [Fact]
        public async Task PowerServiceTradeSubscriberTests_CallGetTradesAsync_DateTimeMaxValue_It_ReturnsValues()
        {
            Assert.NotNull(_sut);
            Assert.IsType<PowerTradeProviderService>(_sut);

            DateTime date = DateTime.MaxValue;
            var powerTrades = await _sut.GetTradesAsync(date);

            Assert.NotNull(powerTrades);
            Assert.IsType<List<PowerTradeDTO>>(powerTrades);
        }

        [Fact]
        public async Task PowerServiceTradeSubscriberTests_CallGetTradesAsync_DateTimeMinValue_It_ThrowsOutOfRangeError()
        {
            Assert.NotNull(_sut);
            Assert.IsType<PowerTradeProviderService>(_sut);

            DateTime date = DateTime.MinValue;
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await _sut.GetTradesAsync(date));
        }
    }
}
