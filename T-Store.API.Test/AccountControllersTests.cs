using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using T_Store.Controllers;
using T_Store.MapperConfig;
using T_Store.Models;
using T_Strore.Business.Services;
using T_Strore.Data;

namespace T_Store.API.Tests;
public class AccountControllersTests
{
    private AccountControllers _sut;
    private Mock<ITransactionServices> _transactionServiceMock;
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MapperConfigStorage>()));
        _transactionServiceMock = new Mock<ITransactionServices>();
        _sut = new AccountControllers(_transactionServiceMock.Object, _mapper);
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


        _transactionServiceMock.Verify(o => o.GetBalanceByAccountId(expectedTransaction.Id), Times.Once);

    }
}
