using IncredibleBackendContracts.Enums;
using Microsoft.Extensions.Logging;
using Moq;
using System.Transactions;
using T_Strore.Business.Exceptions;
using T_Strore.Business.Models;
using T_Strore.Business.Services;

namespace T_Store.Business.Tests.RateServiceTests;

public class RateServiceNegativeTests
{
    private RateService _sut;
    private Mock<ILogger<RateService>> _logger;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<RateService>>();
        _sut = new RateService(_logger.Object);
    }

    [Test]
    public async Task SaveCurrencyRate_RateModelIsEmpty_ThrowServiceUnavailableException()
    {
        //given
        Dictionary<string, decimal> ratesDictionary = null;

        //when, then
        Assert.Throws<ServiceUnavailableException>(() => _sut.SaveCurrencyRate(ratesDictionary));
    }

    [Test]
    public async Task GetCurrencyRate_RateModelIsEmpty_ThrowServiceUnavailableException()
    {
        //given
        Dictionary<string, decimal> ratesDictionary = null;

        var currencyFirst = Currency.USD.ToString();
        var currencySecond = Currency.RUB.ToString();
        
        //when, then
        Assert.Throws<ServiceUnavailableException>(() => _sut.GetCurrencyRate(currencyFirst, currencySecond));
    }
}
