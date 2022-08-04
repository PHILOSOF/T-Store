using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using T_Store.Controllers;
using Moq;
using AutoMapper;
using T_Strore.Business.Services;
using T_Store.Models;
using T_Strore.Data;
using T_Store.Mapper;

public class TransactionControllerTests
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
    public void AddDeposit_WhenValidRequestPassed_ThenCreatedResultRecived()
    {
        // given
        _transactionServiceMock.Setup(o => o.AddDeposit(It.IsAny<TransactionDto>())).Returns(1);
        var transaction = new TransactionRequest()
        {
            AccountId = 1,
            Currency = Currency.USD,
            Amount = 100
        };

        // when
        var actual = _sut.AddDeposit(transaction);

        // then
        var actualResult = actual.Result as CreatedResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        Assert.True((int)actualResult.Value == 1);
        _transactionServiceMock.Verify(o => o.AddDeposit(
            It.Is<TransactionDto>
            (t=>t.AccountId == transaction.AccountId &&
            t.Currency == transaction.Currency &&
            t.Amount == transaction.Amount
            )), Times.Once);
    }

}
