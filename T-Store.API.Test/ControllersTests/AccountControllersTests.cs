using AutoMapper;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using T_Store.Controllers;
using T_Store.MapperConfig;
using T_Store.Models;
using T_Store.Models.Responses;
using T_Strore.Business.Services;
using T_Strore.Data;

namespace T_Store.API.Test.ControllersTests;
public class AccountControllersTests
{
    private AccountsController _sut;
    private Mock<ITransactionServices> _transactionServiceMock;
    private IMapper _mapper;
    private Mock<ILogger<AccountsController>> _logger;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<AccountsController>>();
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MapperConfigStorage>()));
        _transactionServiceMock = new Mock<ITransactionServices>();
        _sut = new AccountsController(_transactionServiceMock.Object, _mapper, _logger.Object);
    }

    [Test]
    public async Task GetBalanceByAccountId_WhenValidRequestPassed_OkReceived()
    {
        // given
        decimal expected = 100;
        var expectedTransaction = new TransactionDto()
        {
            Id = 1,
            AccountId = 1,
            Currency = Currency.USD,
            Amount = 100,
            Date = DateTime.Now,
            TransactionType = TransactionType.Deposit
        };
        _transactionServiceMock.Setup(t => t.GetBalanceByAccountId(expectedTransaction.AccountId)).ReturnsAsync(expected);

        // when
        var actual = await _sut.GetBalanceByAccountId(expectedTransaction.Id);

        // then
        var actualResult = actual.Result as ObjectResult;

        Assert.AreEqual(actualResult.StatusCode, StatusCodes.Status200OK);
        Assert.AreEqual(expected, actualResult.Value);
        Assert.AreEqual(actualResult.Value.GetType(), expected.GetType());
        _transactionServiceMock.Verify(o => o.GetBalanceByAccountId(expectedTransaction.Id), Times.Once);
    }

    [Test]
    public async Task GetTransactionsByAccountId_WhenValidRequestPassed_OkReceived()
    {
        //given
        long expectedAccountId = 1;
        var expectedTransactions = new Dictionary<DateTime, List<TransactionDto>>()
        {
            { new DateTime(2022,05,05), new List<TransactionDto>()
                {
                    new TransactionDto()
                    {
                        Id = 1,
                        AccountId = 1,
                        Currency = Currency.USD,
                        Amount = -100,
                        Date = new DateTime(2022,05,05),
                        TransactionType = TransactionType.Transfer

                    },
                    new TransactionDto()
                    {
                        Id = 2,
                        AccountId = 3,
                        Currency = Currency.RUB,
                        Amount = 200,
                        Date = new DateTime(2022,05,05),
                        TransactionType = TransactionType.Transfer

                    }
                }


            },
            {new DateTime(2021,05,05), new List<TransactionDto>()
                {
                    new TransactionDto()
                    {
                        Id = 3,
                        AccountId = 1,
                        Currency = Currency.USD,
                        Amount = 10,
                        Date = new DateTime(2021,05,05),
                        TransactionType = TransactionType.Withdraw

                    },
                }
            },
        };
        var transferExpected = expectedTransactions[new DateTime(2022, 05, 05)];
        var transactionExpected = expectedTransactions[new DateTime(2021, 05, 05)];
        _transactionServiceMock.Setup(t => t.GetTransactionsByAccountId(expectedAccountId)).ReturnsAsync(expectedTransactions);

        //when
        var actual = await _sut.GetTransactionsByAccountId(expectedAccountId);

        //then
        var actualResult = actual as ObjectResult;
        var actualListResponses = actualResult.Value as List<TransactionResponse>;
        var actualTransfer = actualListResponses[0] as TransferResponse;
        var actualTransaction = actualListResponses[1] as TransactionResponse;

        Assert.AreEqual(actualResult.StatusCode, StatusCodes.Status200OK);
        Assert.AreEqual(actualTransfer.Id, transferExpected[0].Id);
        Assert.AreEqual(actualTransfer.AccountId, transferExpected[0].AccountId);
        Assert.AreEqual(actualTransfer.Date, transferExpected[0].Date);
        Assert.AreEqual(actualTransfer.TransactionType, transferExpected[0].TransactionType);
        Assert.AreEqual(actualTransfer.Amount, transferExpected[0].Amount);
        Assert.AreEqual(actualTransfer.Currency, transferExpected[0].Currency);
        Assert.AreEqual(actualTransfer.RecipientAccountId, transferExpected[1].AccountId);
        Assert.AreEqual(actualTransfer.RecipientAmount, transferExpected[1].Amount);
        Assert.AreEqual(actualTransfer.RecipientCurrency, transferExpected[1].Currency);
        Assert.AreEqual(actualTransfer.RecipientId, transferExpected[1].Id);
        Assert.AreEqual(actualTransaction.Id, transactionExpected[0].Id);
        Assert.AreEqual(actualTransaction.AccountId, transactionExpected[0].AccountId);
        Assert.AreEqual(actualTransaction.Currency, transactionExpected[0].Currency);
        Assert.AreEqual(actualTransaction.Amount, transactionExpected[0].Amount);
        Assert.AreEqual(actualTransaction.Date, transactionExpected[0].Date);
        Assert.AreEqual(actualTransaction.TransactionType, transactionExpected[0].TransactionType);

        _transactionServiceMock.Verify(o => o.GetTransactionsByAccountId(expectedAccountId), Times.Once);
    }
}
