using AutoMapper;
using IncredibleBackendContracts.Enums;
using IncredibleBackendContracts.Requests;
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
public class TransactionControllersTests
{
    private TransactionsController _sut;
    private Mock<ITransactionService> _transactionServiceMock;
    private IMapper _mapper;
    private Mock<ILogger<TransactionsController>> _logger;  

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<TransactionsController>>();
        _mapper = new Mapper(new AutoMapper.MapperConfiguration(cfg => cfg.AddProfile<MapperConfigAPI>()));
        _transactionServiceMock = new Mock<ITransactionService>();
        _sut = new TransactionsController(_transactionServiceMock.Object, _mapper, _logger.Object);
    }

    [Test]
    public async Task AddDeposit_WhenValidRequestPassed_ThenCreatedResultRecived()
    {
        // given
        var expectedId = 1;
        var transaction = new TransactionRequest()
        {
            AccountId = 1,
            Currency = Currency.USD,
            Amount = 100
        };
        _transactionServiceMock.Setup(t => t.AddDeposit(It.Is<TransactionModel>(t => t.AccountId == transaction.AccountId))).ReturnsAsync(1);

        // when
        var actual = await _sut.AddDeposit(transaction);

        // then
        var actualResult = actual.Result as CreatedResult;

        Assert.AreEqual(StatusCodes.Status201Created, actualResult.StatusCode);
        Assert.AreEqual(expectedId, actualResult.Value);
        _transactionServiceMock.Verify(o => o.AddDeposit(
            It.Is<TransactionModel>
            (t => t.AccountId == transaction.AccountId &&
            t.Currency == transaction.Currency &&
            t.Amount == transaction.Amount
            )), Times.Once);
    }

    [Test]
    public async Task AddTransfer_WhenValidRequestPassed_ThenCreatedResultRecived()
    {
        // given
        var expectedIds = new List<long> { 1, 2 };

        var transfer = new TransactionTransferRequest()
        {
            AccountId = 1,
            Currency = Currency.USD,
            Amount = 100,
            RecipientAccountId = 2,
            RecipientCurrency = Currency.RUB
        };
        _transactionServiceMock.Setup(o => o.AddTransfer(It.Is<List<TransactionModel>>(t =>
        t[0].AccountId == transfer.AccountId &&
        t[1].AccountId == transfer.RecipientAccountId)))
        .ReturnsAsync(expectedIds);

        // when
        var actual = await _sut.AddTransfer(transfer);

        // then
        var actualResult = actual.Result as CreatedResult;

        Assert.AreEqual(StatusCodes.Status201Created, actualResult.StatusCode);
        Assert.AreEqual(expectedIds, actualResult.Value);
        _transactionServiceMock.Verify(o => o.AddTransfer(
            It.Is<List<TransactionModel>>
            (t => t[0].AccountId == transfer.AccountId &&
            t[0].Currency == transfer.Currency &&
            t[0].Amount == transfer.Amount &&
            t[1].AccountId == transfer.RecipientAccountId &&
            t[1].Currency == transfer.RecipientCurrency
            )), Times.Once);
    }

    [Test]
    public async Task WithdrawDeposit_WhenValidRequestPassed_ThenCreatedResultRecived()
    {
        // given
        var resultExpected = 1;
        var transaction = new TransactionRequest()
        {
            AccountId = 1,
            Currency = Currency.USD,
            Amount = 100
        };
        _transactionServiceMock.Setup(t => t.Withdraw(It.Is<TransactionModel>(t => t.AccountId == transaction.AccountId))).ReturnsAsync(1);

        // when
        var actual = await _sut.Withdraw(transaction);

        // then
        var actualResult = actual.Result as CreatedResult;

        Assert.AreEqual(StatusCodes.Status201Created, actualResult.StatusCode);
        Assert.AreEqual(resultExpected, actualResult.Value);
        _transactionServiceMock.Verify(o => o.Withdraw(
            It.Is<TransactionModel>
            (t => t.AccountId == transaction.AccountId &&
            t.Currency == transaction.Currency &&
            t.Amount == transaction.Amount
            )), Times.Once);
    }

    [Test]
    public async Task GetTransactionById_WhenValidRequestPassed_OkReceived()
    {
        // given
        var expectedTransaction = new TransactionModel()
        {
            Id = 1,
            AccountId = 1,
            Currency = Currency.USD,
            Amount = 100,
            Date = DateTime.Now,
            TransactionType = TransactionType.Deposit
        };
        _transactionServiceMock.Setup(t => t.GetTransactionById(expectedTransaction.Id)).ReturnsAsync(expectedTransaction);

        // when
        var actual = await _sut.GetTransactionById(expectedTransaction.Id);

        // then
        var actualResult = actual.Result as ObjectResult;
        var actualTransaction = actualResult.Value as TransactionResponse;

        Assert.AreEqual(StatusCodes.Status200OK, actualResult.StatusCode);
        Assert.AreEqual(expectedTransaction.Id, actualTransaction.Id);
        Assert.AreEqual(expectedTransaction.AccountId, actualTransaction.AccountId);
        Assert.AreEqual(expectedTransaction.Currency, actualTransaction.Currency);
        Assert.AreEqual(expectedTransaction.Amount, actualTransaction.Amount);
        Assert.AreEqual(expectedTransaction.Date, actualTransaction.Date);
        Assert.AreEqual(expectedTransaction.TransactionType, actualTransaction.TransactionType);

        _transactionServiceMock.Verify(o => o.GetTransactionById(expectedTransaction.Id), Times.Once);
    }
}
