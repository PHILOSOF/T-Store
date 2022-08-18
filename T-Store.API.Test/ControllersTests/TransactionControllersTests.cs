using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using T_Store.Controllers;
using Moq;
using AutoMapper;
using T_Strore.Business.Services;
using T_Store.Models;
using T_Strore.Data;
using T_Store.MapperConfig;
using NUnit.Framework;

namespace T_Store.API.Test.ControllersTests;
public class TransactionControllersTests
{
    private TransactionsController _sut;
    private Mock<ITransactionServices> _transactionServiceMock;
    private IMapper _mapper;
    

    [SetUp]
    public void Setup()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MapperConfigStorage>()));
        _transactionServiceMock = new Mock<ITransactionServices>();
        _sut = new TransactionsController(_transactionServiceMock.Object, _mapper);
    }

    [Test]
    public async Task AddDeposit_WhenValidRequestPassed_ThenCreatedResultRecived()
    {
        // given
        var transaction = new TransactionRequest()
        {
            AccountId = 1,
            Currency = Currency.USD,
            Amount = 100
        };
        _transactionServiceMock.Setup(t => t.AddDeposit(It.Is<TransactionDto>(t => t.AccountId == transaction.AccountId))).ReturnsAsync(1);

        // when
        var actual = await _sut.AddDeposit(transaction);

        // then
        var actualResult = actual.Result as CreatedResult;

        Assert.AreEqual(actualResult.StatusCode, StatusCodes.Status201Created);
        Assert.AreEqual(actualResult.Value, 1);
        _transactionServiceMock.Verify(o => o.AddDeposit(
            It.Is<TransactionDto>
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
        _transactionServiceMock.Setup(o => o.AddTransfer(It.Is<List<TransactionDto>>(t =>
        t[0].AccountId == transfer.AccountId &&
        t[1].AccountId == transfer.RecipientAccountId)))
        .ReturnsAsync(expectedIds);

        // when
        var actual = await _sut.AddTransfer(transfer);

        // then
        var actualResult = actual.Result as CreatedResult;

        Assert.AreEqual(actualResult.StatusCode, StatusCodes.Status201Created);
        Assert.AreEqual(actualResult.Value, expectedIds);
        _transactionServiceMock.Verify(o => o.AddTransfer(
            It.Is<List<TransactionDto>>
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
        var transaction = new TransactionRequest()
        {
            AccountId = 1,
            Currency = Currency.USD,
            Amount = 100
        };
        _transactionServiceMock.Setup(t => t.Withdraw(It.Is<TransactionDto>(t => t.AccountId == transaction.AccountId))).ReturnsAsync(1);

        // when
        var actual = await _sut.Withdraw(transaction);

        // then
        var actualResult = actual.Result as CreatedResult;

        Assert.AreEqual(actualResult.StatusCode, StatusCodes.Status201Created);
        Assert.AreEqual(actualResult.Value, 1);
        _transactionServiceMock.Verify(o => o.Withdraw(
            It.Is<TransactionDto>
            (t => t.AccountId == transaction.AccountId &&
            t.Currency == transaction.Currency &&
            t.Amount == transaction.Amount
            )), Times.Once);
    }

    [Test]
    public async Task GetTransactionById_WhenValidRequestPassed_OkReceived()
    {
        // given
        var expectedTransaction = new TransactionDto()
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

        Assert.AreEqual(actualResult.StatusCode, StatusCodes.Status200OK);
        Assert.AreEqual(actualTransaction.Id, expectedTransaction.Id);
        Assert.AreEqual(actualTransaction.AccountId, expectedTransaction.AccountId);
        Assert.AreEqual(actualTransaction.Currency, expectedTransaction.Currency);
        Assert.AreEqual(actualTransaction.Amount, expectedTransaction.Amount);
        Assert.AreEqual(actualTransaction.Date, expectedTransaction.Date);
        Assert.AreEqual(actualTransaction.TransactionType, expectedTransaction.TransactionType);

        _transactionServiceMock.Verify(o => o.GetTransactionById(expectedTransaction.Id), Times.Once);
    }
}
