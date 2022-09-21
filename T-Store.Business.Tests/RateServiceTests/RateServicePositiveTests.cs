using IncredibleBackendContracts.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.QualityTools.Testing.Fakes.Shims;
using Moq;
using T_Store.Business.Tests.CaseSource;
using T_Strore.Business.Models;
using T_Strore.Business.Services;

namespace T_Store.Business.Tests.RateServiceTests;

public class RateServicePositiveTests
{
    private RateService _sut;
    private Mock<ILogger<RateService>> _logger;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<RateService>>();
        _sut = new RateService(_logger.Object);

       
        RateModel.CurrencyRates = new Dictionary<string, decimal>()
        {
            { "EUR", 0.9958m },
            { "RUB", 60.47m },
            { "JPY", 142.47m },
            { "AMD", 405.3m },
            { "BGN", 1.95m },
            { "RSD", 116.74m },
            { "CNY", 6.92m },
        };
        RateModel.BaseCurrency = Currency.USD.ToString();
    }


    [Test]
    public async Task SaveCurrencyRate_ValidRequestPassed_CurrencyRateSaveInClass()
    {

        //given
        RateModel.CurrencyRates = null;
        RateModel.BaseCurrency = null;
        var expectedRates = new Dictionary<string, decimal>()
        {
           { "EUR", 0.9958m },
           { "RUB", 60.47m },
           { "JPY", 142.47m },
           { "AMD", 405.3m },
           { "BGN", 1.95m },
           { "RSD", 116.74m },
           { "CNY", 6.92m },
        };

        var ratesDictionary = new Dictionary<string, decimal>()
        {
            { "USDEUR", 0.9958m },
            { "USDRUB", 60.47m },
            { "USDJPY", 142.47m },
            { "USDAMD", 405.3m },
            { "USDBGN", 1.95m },
            { "USDRSD", 116.74m },
            { "USDCNY", 6.92m },
        };

        //when
        _sut.SaveCurrencyRate(ratesDictionary);

        //then
        Assert.That(Currency.USD.ToString(), Is.EqualTo(RateModel.BaseCurrency));
        Assert.That(expectedRates, Is.EqualTo(RateModel.CurrencyRates));
    }


    [TestCaseSource(typeof(PairsWithoutUsdSource))]
    public async Task GetCurrencyRate_ValidRequestPassedConvertWithoutBaseCurrency_CurrencyRateReturned(Currency first, Currency second)
    {
        //given

        var currencyFirst = first.ToString();
        var currencySecond = second.ToString();

        var expectedRate = RateModel.CurrencyRates[currencySecond] / RateModel.CurrencyRates[currencyFirst];
                                    // exemple calculating the cross rate,  EURJPY / = [USD / JPY] / [USD / EUR], if base USD
        //when
        var actualRate = _sut.GetCurrencyRate(currencyFirst, currencySecond);

        //then
        Assert.That(actualRate, Is.EqualTo(expectedRate));

    }


    [TestCaseSource(typeof(PairsWithUsdSource))]
    public async Task GetCurrencyRate_ValidRequestPassedConvertWithBaseCurrencySecond_CurrencyRateReturned(Currency first)
    {
        //given
        var currencyFirst = first.ToString();
        var currencySecondBase = Currency.USD.ToString();
        var expectedRate = RateModel.CurrencyRates[first.ToString()];
      
        //when
        var actualRate = _sut.GetCurrencyRate(currencyFirst, currencySecondBase);

        //then
        Assert.That(actualRate, Is.EqualTo(expectedRate));
    }


    [TestCaseSource(typeof(PairsWithUsdSource))]
    public async Task GetCurrencyRate_ValidRequestPassedConvertWithBaseCurrencyFirst_CurrencyRateReturned(Currency second)
    {
        //given

        var currencySecond = second.ToString();
        var currencyFirstBase = Currency.USD.ToString();

        var expectedRate = RateModel.CurrencyRates[currencySecond.ToString()];

        //when
        var actualRate = _sut.GetCurrencyRate(currencyFirstBase, currencySecond);

        //then
        Assert.That(actualRate, Is.EqualTo(expectedRate));

    }
}