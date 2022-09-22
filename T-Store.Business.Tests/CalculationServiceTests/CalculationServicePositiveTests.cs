using Microsoft.Extensions.Logging;
using Moq;
using T_Store.Business.Tests.CaseSource;
using T_Strore.Business.Models;
using T_Strore.Business.Services;
using IncredibleBackendContracts.Enums;
using T_Strore.Business.Services.Interfaces;


namespace T_Store.Business.Tests.CalculationServicePositiveTests;

public class CalculationServicesPositiveTests
{
    private CalculationService _sut;
    private Mock<ILogger<CalculationService>> _logger;
    private Mock<IRateService> _rateServiceMock;
 
    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<CalculationService>>();
        _rateServiceMock = new Mock<IRateService>();
        _sut = new CalculationService(_logger.Object, _rateServiceMock.Object);
        RateModel.BaseCurrency = Currency.USD.ToString();
    }


    [TestCaseSource(typeof(PairsWithUsdSource))]
    public async Task ConvertCurrency_ValidRequestPassed_ListTransferModelReturnedWereConverUsdToCurrency(Currency recipient)
    {
        //given
        var ratesDictionary = new Dictionary<string,decimal>()
        {
            { "EUR", 0.9958m },
            { "RUB", 60.47m },
            { "JPY", 142.47m },
            { "AMD", 405.3m },
            { "BGN", 1.95m },
            { "RSD", 116.74m },
            { "CNY", 6.92m },
        };

        var transferModel = new List<TransactionModel>()
        {
            new()
            {
                Id = 1,
                AccountId = 1,
                Date = new DateTime(2022, 05, 05),
                Amount = 10,
                Currency = Currency.USD

            },
            new()
            {
                Id = 2,
                AccountId = 2,
                Date = new DateTime(2022, 05, 05),
                Currency = recipient

            }
        };
        _rateServiceMock.Setup(t => t.GetCurrencyRate(transferModel[0].Currency.ToString(), transferModel[1].Currency.ToString()))
            .Returns(ratesDictionary[recipient.ToString()]);

        transferModel[0].Amount = 0 - transferModel[0].Amount;
        transferModel[1].Amount = transferModel[0].Amount * ratesDictionary[transferModel[1].Currency.ToString()];

        //when
        var actual = await _sut.ConvertCurrency(transferModel);

        //then
        Assert.AreEqual(transferModel, actual);
    }


    [TestCaseSource(typeof(PairsWithUsdSource))]
    public async Task ConvertCurrency_ValidRequestPassed_ListTransferModelReturnedWereConverСurrencyToUsd(Currency sender)
    {
        //given
        var ratesDictionary = new Dictionary<string, decimal>()
        {
            { "EUR", 0.9958m },
            { "RUB", 60.47m },
            { "JPY", 142.47m },
            { "AMD", 405.3m },
            { "BGN", 1.95m },
            { "RSD", 116.74m },
            { "CNY", 6.92m },
        };

        var transferModel = new List<TransactionModel>()
        {
            new()
            {
                Id = 1,
                AccountId = 1,
                Date = new DateTime(2022, 05, 05),
                Amount = 10,
                Currency = sender

            },
            new()
            {
                Id = 2,
                AccountId = 2,
                Date = new DateTime(2022, 05, 05),
                Currency = Currency.USD

            }
        };
        _rateServiceMock.Setup(t => t.GetCurrencyRate(transferModel[0].Currency.ToString(), transferModel[1].Currency.ToString()))
            .Returns(ratesDictionary[sender.ToString()]);

        var senderAmountExpected = 0 - transferModel[0].Amount;
        var recipientAmountExpected = transferModel[0].Amount / ratesDictionary[sender.ToString()];

        //when
        var actual = await _sut.ConvertCurrency(transferModel);

        //then
        Assert.AreEqual(transferModel, actual);
    }


    [TestCaseSource(typeof(PairsWithoutUsdSource))]
    public async Task ConvertCurrency_ValidRequestPassed_ListTransferModelReturnedWereConverPairsBesidesUsd(Currency sender, Currency recipient)
    {
        //given
        var ratesDictionary = new Dictionary<string, decimal>()
        {
            { "EUR", 0.9958m },
            { "RUB", 60.47m },
            { "JPY", 142.47m },
            { "AMD", 405.3m },
            { "BGN", 1.95m },
            { "RSD", 116.74m },
            { "CNY", 6.92m },
        };

        var transferModel = new List<TransactionModel>()
        {
            new()
            {
                Id = 1,
                AccountId = 1,
                Date = new DateTime(2022, 05, 05),
                Amount = 1000,
                Currency = sender

            },
            new()
            {
                Id = 2,
                AccountId = 2,
                Date = new DateTime(2022, 05, 05),
                Currency = recipient

            }
        };
        _rateServiceMock.Setup(t => t.GetCurrencyRate(transferModel[0].Currency.ToString(), transferModel[1].Currency.ToString()))
            .Returns(ratesDictionary[recipient.ToString()]/ ratesDictionary[sender.ToString()]);

        transferModel[0].Amount = 0 - transferModel[0].Amount;
        transferModel[1].Amount = Math.Round(transferModel[0].Amount / ratesDictionary[sender.ToString()] * ratesDictionary[recipient.ToString()],
            4, MidpointRounding.ToNegativeInfinity);

        //when
        var actual = await _sut.ConvertCurrency(transferModel);

        //then
        Assert.AreEqual(transferModel, actual); 
    }
}
