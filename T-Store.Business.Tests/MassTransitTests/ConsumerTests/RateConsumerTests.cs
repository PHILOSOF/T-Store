using IncredibleBackendContracts.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using T_Strore.Business.Consumers;
using T_Strore.Business.Services.Interfaces;

namespace T_Store.Business.Tests.MassTransitTests.ConsumerTests;

public class RateConsumerTests
{
    private RateConsumer _sut;
    private Mock<ILogger<RateConsumer>> _logger;
    private Mock<IRateService> _rateServiceMock;
 

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<RateConsumer>>();
        _rateServiceMock = new Mock<IRateService>();
        _sut = new RateConsumer(_logger.Object, _rateServiceMock.Object);

    }

    [Test]
    public async Task Consume_ValidRequestPassed_CurrencyRateSave()
    {
        //given
        var rates = new Dictionary <string, decimal>()
        {
            {"USDRUB",1}
        };
        var context = Mock.Of<ConsumeContext<NewRatesEvent>>(c=>c.Message.Rates == rates);

        //when
        _sut.Consume(context);

        //then
        _rateServiceMock.Verify(r => r.SaveCurrencyRate(context.Message.Rates), Times.Once);
    }
}
