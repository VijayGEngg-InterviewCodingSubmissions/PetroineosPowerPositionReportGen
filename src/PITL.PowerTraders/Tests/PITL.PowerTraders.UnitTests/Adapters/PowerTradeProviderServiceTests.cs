using Microsoft.Extensions.Logging;
using NSubstitute;
using PITL.PowerTraders.Interfaces;
using PITL.PowerTraders.Models.DomainModels;
using PITL.PowerTraders.PowerServiceConsumer;
using PITL.PowerTraders.PowerServiceConsumer.Mapper;
using Services;

namespace PITL.PowerTraders.UnitTests.Adapters
{
    public class PowerTradeProviderServiceTests
    {
        private IPowerTradeProviderService _sut;
        private IMapper<PowerTrade, PowerTradeDTO> _mockMapper;

        public PowerTradeProviderServiceTests() : base()
        {
            ILogger<PowerTradeProviderService> sutLogger = Substitute.For<ILogger<PowerTradeProviderService>>();
            _mockMapper = Substitute.For<PowerTradeMapper>();
            _sut = new PowerTradeProviderService(sutLogger, _mockMapper);
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
    }
}
