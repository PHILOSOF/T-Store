using Microsoft.Extensions.Logging;
using Moq;
using T_Store.Business.Tests.CaseSource;
using T_Strore.Business.Models;
using T_Strore.Business.Services;
using IncredibleBackendContracts.Enums;

namespace T_Store.Business.Tests.CalculationServicePositiveTests;

public class CalculationServicesPositiveTests
{
    private CalculationService _sut;
    private Mock<ILogger<CalculationService>> _logger;


    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<CalculationService>>();
        _sut = new CalculationService(_logger.Object);
    }

    [TestCase(Currency.EUR)]
    [TestCase(Currency.RUB)]
    [TestCase(Currency.JPY)]
    [TestCase(Currency.AMD)]
    [TestCase(Currency.BGN)]
    [TestCase(Currency.RSD)]
    public async Task ConvertCurrency_ValidRequestPassed_ListTransferModelReturnedWereConverUsdToCurrency(Currency recipient)
    {
        //given
        var ratesList = new List<decimal>()
        {
            0.98m,  //EUR
            61.88m, //RUB
            1,      //USD
            134.61m,//JPY
            406.61m,//AMD
            1.92m,  //BGN
            115.02m //RSD
        };
        var ratesResult = Enum.GetValues(typeof(Currency))
             .Cast<Currency>()
             .ToDictionary(t => (Currency.USD, t), b => ratesList[(int)b - 1]);

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
        var senderAmountExpected = 0 - transferModel[0].Amount;
        var recipientAmountExpected = transferModel[0].Amount * ratesResult[(Currency.USD, recipient)];
        //when
        var actual = await _sut.ConvertCurrency(transferModel);

        //then
        Assert.AreEqual(transferModel[0].Id, transferModel[0].Id);
        Assert.AreEqual(transferModel[0].Date, transferModel[0].Date);
        Assert.AreEqual(transferModel[0].Currency, transferModel[0].Currency);
        Assert.AreEqual(transferModel[1].Id, transferModel[1].Id);
        Assert.AreEqual(transferModel[1].Date, transferModel[1].Date);
        Assert.AreEqual(transferModel[1].Currency, transferModel[1].Currency);
        Assert.AreEqual(actual[0].Amount, senderAmountExpected);
        Assert.AreEqual(actual[1].Amount, recipientAmountExpected);
        Assert.AreEqual(actual[0].Date, actual[1].Date);
    }


    [TestCase(Currency.EUR)]
    [TestCase(Currency.RUB)]
    [TestCase(Currency.JPY)]
    [TestCase(Currency.AMD)]
    [TestCase(Currency.BGN)]
    [TestCase(Currency.RSD)]
    public async Task ConvertCurrency_ValidRequestPassed_ListTransferModelReturnedWereConverСurrencyToUsd(Currency sender)
    {
        //given
        var ratesList = new List<decimal>()
        {
            0.98m,  //EUR
            61.88m, //RUB
            1,      //USD
            134.61m,//JPY
            406.61m,//AMD
            1.92m,  //BGN
            115.02m //RSD
        };
        var ratesResult = Enum.GetValues(typeof(Currency))
             .Cast<Currency>()
             .ToDictionary(t => (Currency.USD, t), b => ratesList[(int)b - 1]);

        var transferModel = new List<TransactionModel>()
        {
            new()
            {
                Id = 1,
                AccountId = 1,
                Date = new DateTime(2022, 05, 05),
                Amount = 100,
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
        var senderAmountExpected = 0 - transferModel[0].Amount;
        var recipientAmountExpected = transferModel[0].Amount / ratesResult[(Currency.USD, sender)];

        //when
        var actual = await _sut.ConvertCurrency(transferModel);

        //then
        Assert.AreEqual(transferModel[0].Id, transferModel[0].Id);
        Assert.AreEqual(transferModel[0].Date, transferModel[0].Date);
        Assert.AreEqual(transferModel[0].Currency, transferModel[0].Currency);
        Assert.AreEqual(transferModel[1].Id, transferModel[1].Id);
        Assert.AreEqual(transferModel[1].Date, transferModel[1].Date);
        Assert.AreEqual(transferModel[1].Currency, transferModel[1].Currency);
        Assert.AreEqual(actual[0].Amount, senderAmountExpected);
        Assert.AreEqual(actual[1].Amount, recipientAmountExpected);
        Assert.AreEqual(actual[0].Date, actual[1].Date);

    }

    [TestCaseSource(typeof(PairsBesidesUsdSource))]
    public async Task ConvertCurrency_ValidRequestPassed_ListTransferModelReturnedWereConverPairsBesidesUsd(Currency sender, Currency recipient)
    {
        //given
        var ratesList = new List<decimal>()
        {
            0.98m,  //EUR
            61.88m, //RUB
            1,      //USD
            134.61m,//JPY
            406.61m,//AMD
            1.92m,  //BGN
            115.02m //RSD
        };
        var ratesResult = Enum.GetValues(typeof(Currency))
             .Cast<Currency>()
             .ToDictionary(t => (Currency.USD, t), b => ratesList[(int)b - 1]);


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
        var senderAmountExpected = 0 - transferModel[0].Amount;
        var recipientAmountExpected = transferModel[0].Amount / ratesResult[(Currency.USD, sender)] * ratesResult[(Currency.USD, recipient)];
        //when
        var actual = await _sut.ConvertCurrency(transferModel);

        //then
        Assert.AreEqual(transferModel[0].Id, transferModel[0].Id);
        Assert.AreEqual(transferModel[0].Date, transferModel[0].Date);
        Assert.AreEqual(transferModel[0].Currency, transferModel[0].Currency);
        Assert.AreEqual(transferModel[1].Id, transferModel[1].Id);
        Assert.AreEqual(transferModel[1].Date, transferModel[1].Date);
        Assert.AreEqual(transferModel[1].Currency, transferModel[1].Currency);
        Assert.AreEqual(actual[0].Amount, senderAmountExpected);
        Assert.AreEqual(actual[1].Amount, recipientAmountExpected);
        Assert.AreEqual(actual[0].Date, actual[1].Date);

    }
}
