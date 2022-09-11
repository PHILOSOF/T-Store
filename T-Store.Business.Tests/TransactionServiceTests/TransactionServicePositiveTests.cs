using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using T_Strore.Business.Exceptions;
using T_Strore.Business.MapperConfiguration;
using T_Strore.Business.Models;
using T_Strore.Business.Services;
using T_Strore.Data;
using T_Strore.Data.Repository;
using IncredibleBackendContracts.Enums;

namespace T_Store.Business.Tests.TransactionServiceTests;

public class TransactionServicePositiveTests
{
    private TransactionService _sut;
    private Mock<ITransactionRepository> _transactionRepositoryMock;
    private Mock<ICalculationService> _calculationService;
    private Mock<ILogger<TransactionService>> _logger;
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MapperConfigBusiness>()));
        _logger = new Mock<ILogger<TransactionService>>();
        _transactionRepositoryMock = new Mock<ITransactionRepository>();
        _calculationService = new Mock<ICalculationService>();
        _sut = new TransactionService(_transactionRepositoryMock.Object, _calculationService.Object, _mapper, _logger.Object, null);
    }

    [Test]
    public async Task AddDeposit_ValidRequestPassed_AddTransactionAndIdReturned()
    {
        //given
        var expectedId = 1;

        var transaction = new TransactionModel()
        {
            Id = 1,
            AccountId = 1,
            Amount = 10,
            Currency = Currency.USD

        };


        _transactionRepositoryMock.Setup(t => t.AddTransaction(It.Is<TransactionDto>(t => t.Id == transaction.Id)))
        .ReturnsAsync(1);

        //when
        var actualId = await _sut.AddDeposit(transaction);

        //then

        Assert.AreEqual(actualId, expectedId);
        _transactionRepositoryMock.Verify(t => t.AddTransaction(It.Is<TransactionDto>(c =>
        c.Id == transaction.Id &&
        c.Currency == transaction.Currency &&
        c.TransactionType == transaction.TransactionType &&
        c.AccountId == transaction.AccountId &&
        c.Amount == transaction.Amount)), Times.Once);
    }


    [Test]
    public async Task WithdrawDeposit_ValidRequestPassed_WithdrawAndIdReturned()
    {
        //given


        var transaction = new TransactionModel()
        {
            Id = 1,
            AccountId = 1,
            Amount = 10,
            Currency = Currency.EUR
        };

        _transactionRepositoryMock.Setup(t => t.GetBalanceByAccountId(transaction.Id))
         .ReturnsAsync(100);
        _transactionRepositoryMock.Setup(t => t.AddTransaction(It.Is<TransactionDto>(t => t.Id == transaction.Id)))
        .ReturnsAsync(transaction.Id);


        //when
        var actual = await _sut.Withdraw(transaction);

        //then
        Assert.AreEqual(actual, transaction.Id);
        _transactionRepositoryMock.Verify(t => t.AddTransaction(It.Is<TransactionDto>(t =>
        t.Id == transaction.Id &&
        t.Currency == transaction.Currency &&
        t.TransactionType == transaction.TransactionType &&
        t.AccountId == transaction.AccountId &&
        t.Amount == transaction.Amount)), Times.Once);
        _transactionRepositoryMock.Verify(t => t.GetBalanceByAccountId(transaction.Id), Times.Once);
    }

    [Test]
    public async Task AddTransfer_ValidRequestPassed_AddTransferAndIdReturned()
    {
        //given
        var expectedIds = new List<long> { 1, 2 };
        var transfers = new List<TransactionModel>()
        {
            new TransactionModel()
            {
                Id = 1,
                AccountId = 1,
                Amount = 10,

                Currency=Currency.EUR
            },
            new TransactionModel()
            {
                Id = 2,
                AccountId = 2,
                Currency=Currency.RUB
            }
        };

        var convertModels = new List<TransactionModel>()
        {
             new TransactionModel()
             {
                Id = 1,
                AccountId = 1,
                Date=new DateTime(2022,05,05),
                Amount = -10,
                Currency=Currency.EUR
             },
            new TransactionModel()
            {
                Id = 2,
                AccountId = 2,
                Date=new DateTime(2022,05,05),
                Amount = 20,
                Currency=Currency.RUB
            }
        };

        _calculationService.Setup(c => c.ConvertCurrency(transfers))
         .ReturnsAsync(convertModels);

        _transactionRepositoryMock.Setup(t => t.AddTransferTransactions(It.Is<List<TransactionDto>>(t =>
        t[0].Id == convertModels[0].Id &&
        t[1].Id == convertModels[1].Id)))
         .ReturnsAsync(expectedIds);

        _transactionRepositoryMock.Setup(t => t.GetBalanceByAccountId(transfers[0].AccountId))
        .ReturnsAsync(100);

        //when
        var actual = await _sut.AddTransfer(transfers);

        //then
        Assert.AreEqual(expectedIds, actual);
        _transactionRepositoryMock.Verify(t => t.AddTransferTransactions(It.Is<List<TransactionDto>>(c =>
        c[0].Id == convertModels[0].Id &&
        c[0].Currency == convertModels[0].Currency &&
        c[0].TransactionType == convertModels[0].TransactionType &&
        c[0].AccountId == convertModels[0].AccountId &&
        c[0].Amount == convertModels[0].Amount &&
        c[1].Id == convertModels[1].Id &&
        c[1].Currency == convertModels[1].Currency &&
        c[1].TransactionType == convertModels[1].TransactionType &&
        c[1].AccountId == convertModels[1].AccountId &&
        c[1].Amount == convertModels[1].Amount)), Times.Once);
        _calculationService.Verify(t => t.ConvertCurrency(transfers), Times.Once);
        _transactionRepositoryMock.Verify(t => t.GetBalanceByAccountId(transfers[0].AccountId), Times.Once);
    }

    [Test]
    public async Task GetBalanceByAccountId_ValidRequestPassed_BalanceReturned()
    {
        //given
        var expectedAcoountId = 1;
        decimal expectedBalance = 100;
        _transactionRepositoryMock.Setup(t => t.GetBalanceByAccountId(expectedAcoountId))
        .ReturnsAsync(100);

        //when
        var actual = await _sut.GetBalanceByAccountId(expectedAcoountId);

        //then
        Assert.AreEqual(actual.GetType(), typeof(decimal));
        Assert.AreEqual(expectedBalance, actual);
        _transactionRepositoryMock.Verify(t => t.GetBalanceByAccountId(expectedAcoountId), Times.Once);
    }

    [Test]
    public async Task GetBalanceByAccountId_BalanceIsZerro_ZeroReturned()
    {
        //given
        var expectedAcoountId = 1;
        decimal expectedBalance = 0;

        //when
        var actual = await _sut.GetBalanceByAccountId(expectedAcoountId);

        //then
        Assert.AreEqual(actual.GetType(), typeof(decimal));
        Assert.AreEqual(expectedBalance, actual);
        _transactionRepositoryMock.Verify(t => t.GetBalanceByAccountId(expectedAcoountId), Times.Once);
    }

    [Test]
    public async Task GetTransactionById_ValidRequestPassed_TransactionReturned()
    {
        //given
        var transaction = new TransactionModel()
        {
            Id = 1,
            AccountId = 1,
            Date = new DateTime(2022, 05, 05),
            Amount = 10,
            TransactionType = TransactionType.Deposit,
            Currency = Currency.EUR
        };
        var transactionDto = _mapper.Map<TransactionDto>(transaction);
        _transactionRepositoryMock.Setup(t => t.GetTransactionById(transactionDto.Id))
        .ReturnsAsync(transactionDto);

        //when
        var actual = await _sut.GetTransactionById(transaction.Id);

        //then
        Assert.AreEqual(actual.Id, transaction.Id);
        Assert.AreEqual(actual.AccountId, transaction.AccountId);
        Assert.AreEqual(actual.Date, transaction.Date);
        Assert.AreEqual(actual.Amount, transaction.Amount);
        Assert.AreEqual(actual.TransactionType, transaction.TransactionType);
        Assert.AreEqual(actual.Currency, transaction.Currency);
        _transactionRepositoryMock.Verify(t => t.GetTransactionById(transactionDto.Id), Times.Once);
    }

    [Test]
    public async Task GetTransactionsById_ValidRequestPassed_TransactionsReturned()
    {
        //given
        var transactions = new List<TransactionModel>()
        {
            new()
            {
                Id = 1,
                AccountId = 1,
                Date = new DateTime(2022, 05, 05),
                Amount = -10,
                TransactionType = TransactionType.Transfer,
                Currency = Currency.EUR
            },
            new()
            {
                Id = 2,
                AccountId = 2,
                Date = new DateTime(2022, 05, 05),
                Amount = 100,
                TransactionType = TransactionType.Transfer,
                Currency = Currency.RUB
            },
            new()
            {
                Id = 3,
                AccountId = 3,
                Date = new DateTime(2022, 06, 05),
                Amount = 50,
                TransactionType = TransactionType.Deposit,
                Currency = Currency.JPY
            },
            new()
            {
                Id = 4,
                AccountId = 3,
                Date = new DateTime(2022, 07, 05),
                Amount = -50,
                TransactionType = TransactionType.Withdraw,
                Currency = Currency.JPY
            }
        };
        var transactionDtos = _mapper.Map<List<TransactionModel>, List<TransactionDto>>(transactions);
        _transactionRepositoryMock.Setup(t => t.GetAllTransactionsByAccountId(transactions[0].AccountId))
        .ReturnsAsync(transactionDtos);

        //when
        var actual = _sut.GetTransactionsByAccountId(transactions[0].AccountId);
        var actualWithdraw = actual.Result[transactions[3].Date];
        var actualDeposit = actual.Result[transactions[2].Date];
        var actualTransfer = actual.Result[transactions[1].Date];

        //then
        Assert.AreEqual(actualWithdraw[0], transactions[3]);
        Assert.AreEqual(actualDeposit[0], transactions[2]);
        Assert.AreEqual(actualTransfer[0], transactions[0]);
        Assert.AreEqual(actualTransfer[1], transactions[1]);
        _transactionRepositoryMock.Verify(t => t.GetAllTransactionsByAccountId(transactions[0].Id), Times.Once);
    }
}