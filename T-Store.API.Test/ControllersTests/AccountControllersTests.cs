using AutoMapper;
using IncredibleBackendContracts.Enums;
using IncredibleBackendContracts.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using T_Store.Controllers;
using T_Store.MapperConfiguration;
using T_Strore.Business.Models;
using T_Strore.Business.Services;

namespace T_Store.API.Test.ControllersTests;
public class AccountControllersTests
{
    private AccountsController _sut;
    private Mock<ITransactionService> _transactionServiceMock;
    private IMapper _mapper;
    private Mock<ILogger<AccountsController>> _logger;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<AccountsController>>();
        _mapper = new Mapper(new AutoMapper.MapperConfiguration(cfg => cfg.AddProfile<MapperConfigAPI>()));
        _transactionServiceMock = new Mock<ITransactionService>();
        _sut = new AccountsController(_transactionServiceMock.Object, _mapper, _logger.Object);
    }

    [Test]
    public async Task GetBalanceByAccountId_WhenValidRequestPassed_OkReceived()
    {
        // given
        decimal expected = 100;
        var expectedTransaction = new TransactionModel()
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

        Assert.AreEqual(StatusCodes.Status200OK, actualResult.StatusCode );
        Assert.AreEqual(expected, actualResult.Value);
        Assert.AreEqual(expected.GetType(), actualResult.Value.GetType());
        _transactionServiceMock.Verify(o => o.GetBalanceByAccountId(expectedTransaction.Id), Times.Once);
    }

    [Test]
    public async Task GetTransactionsByAccountId_WhenValidRequestPassed_OkReceived()
    {
        //given
        long expectedAccountId = 1;
        var expectedTransactions = new Dictionary<DateTime, List<TransactionModel>>()
        {
            { 
                new DateTime(2022,05,05), 
                new List<TransactionModel>()
                {
                    new TransactionModel()
                    {
                        Id = 1,
                        AccountId = 1,
                        Currency = Currency.USD,
                        Amount = -100,
                        Date = new DateTime(2022,05,05),
                        TransactionType = TransactionType.Transfer

                    },
                    new TransactionModel()
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
            {new DateTime(2021,05,05), new List<TransactionModel>()
                {
                    new TransactionModel()
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

        Assert.AreEqual(StatusCodes.Status200OK, actualResult.StatusCode);
        Assert.AreEqual(transferExpected[0].Id, actualTransfer.Id);
        Assert.AreEqual(transferExpected[0].AccountId, actualTransfer.AccountId);
        Assert.AreEqual(transferExpected[0].Date, actualTransfer.Date);
        Assert.AreEqual(transferExpected[0].TransactionType, actualTransfer.TransactionType);
        Assert.AreEqual(transferExpected[0].Amount, actualTransfer.Amount);
        Assert.AreEqual(transferExpected[0].Currency, actualTransfer.Currency);
        Assert.AreEqual(transferExpected[1].AccountId, actualTransfer.RecipientAccountId);
        Assert.AreEqual(transferExpected[1].Amount, actualTransfer.RecipientAmount);
        Assert.AreEqual(transferExpected[1].Currency, actualTransfer.RecipientCurrency);
        Assert.AreEqual(transferExpected[1].Id, actualTransfer.RecipientId);
        Assert.AreEqual(transactionExpected[0].Id, actualTransaction.Id);
        Assert.AreEqual(transactionExpected[0].AccountId, actualTransaction.AccountId);
        Assert.AreEqual(transactionExpected[0].Currency, actualTransaction.Currency);
        Assert.AreEqual(transactionExpected[0].Amount, actualTransaction.Amount);
        Assert.AreEqual(transactionExpected[0].Date, actualTransaction.Date);
        Assert.AreEqual(transactionExpected[0].TransactionType, actualTransaction.TransactionType);

        _transactionServiceMock.Verify(o => o.GetTransactionsByAccountId(expectedAccountId), Times.Once);
    }
}
