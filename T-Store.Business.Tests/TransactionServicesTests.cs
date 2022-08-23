using Microsoft.Extensions.Logging;
using Moq;
using T_Strore.Business.Exceptions;
using T_Strore.Business.Services;
using T_Strore.Data;
using T_Strore.Data.Repository;

namespace T_Store.Business.Tests;

public class TransactionServicesTests
{
    private TransactionServices _sut;
    private Mock<ITransactionRepository> _transactionRepositoryMock;
    private Mock<ICalculationServices> _calculationService;
    private Mock<ILogger<TransactionServices>> _logger;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<TransactionServices>>();
        _transactionRepositoryMock = new Mock<ITransactionRepository>();
        _calculationService = new Mock<ICalculationServices>();
        _sut = new TransactionServices(_transactionRepositoryMock.Object, _calculationService.Object, _logger.Object);
    }

    [Test]
    public async Task AddDeposit_ValidRequestPassed_AddTransactionAndIdReturned()
    {
        //given
        var expectedId = 1;

        var transaction = new TransactionDto()
        {

            AccountId = 1,
            Amount = 10,
            Currency = Currency.USD

        };
        _transactionRepositoryMock.Setup(t => t.AddTransaction(It.Is<TransactionDto>(t => t == transaction)))
        .ReturnsAsync(1);

        //when
        var actualId = await _sut.AddDeposit(transaction);

        //then

        Assert.AreEqual(actualId, expectedId);
        _transactionRepositoryMock.Verify(t => t.AddTransaction(transaction), Times.Once);
    }


    [Test]
    public async Task WithdrawDeposit_ValidRequestPassed_WithdrawAndIdReturned()
    {
        //given


        var transaction = new TransactionDto()
        {
            Id = 1,
            AccountId = 1,
            Amount = 10,
            Currency = Currency.EUR
        };

        _transactionRepositoryMock.Setup(t => t.GetBalanceByAccountId(transaction.Id))
         .ReturnsAsync(100);
        _transactionRepositoryMock.Setup(t => t.AddTransaction(It.Is<TransactionDto>(t => t == transaction)))
        .ReturnsAsync(transaction.Id);


        //when
        var actual = await _sut.Withdraw(transaction);

        //then
        Assert.AreEqual(actual, transaction.Id);
        _transactionRepositoryMock.Verify(t => t.AddTransaction(transaction), Times.Once);
    }

    [Test]
    public async Task WithdrawDeposit_BalanceLessRequested_ThrowBadRequestException()
    {
        //given
        var realBalance = 100;
        var transactionWithdraw = new TransactionDto()
        {
            Id = 1,
            AccountId = 1,
            Amount = 1000,
            Currency = Currency.EUR
        };

        _transactionRepositoryMock.Setup(t => t.GetBalanceByAccountId(transactionWithdraw.Id))
         .ReturnsAsync(realBalance);

        //when,then
        Assert.ThrowsAsync<BadRequestException>(() => _sut.Withdraw(transactionWithdraw));

        _transactionRepositoryMock.Verify(t => t.GetBalanceByAccountId(transactionWithdraw.Id), Times.Once);
        _transactionRepositoryMock.Verify(t => t.AddTransaction(It.IsAny<TransactionDto>()), Times.Never);
    }
    [Test]
    public async Task AddTransfer_ValidRequestPassed_AddTransferAndIdReturned()
    {
        //given
        var expecteIds = new List<long> { 1, 2 };
        var transfers = new List<TransactionDto>()
        {
            new TransactionDto()
            {
                Id = 1,
                AccountId = 1,
                Amount = 10,

                Currency=Currency.EUR
            },
            new TransactionDto()
            {
                Id = 2,
                AccountId = 2,
                Currency=Currency.RUB
            }
        };

        var covertModels = new List<TransactionDto>()
        {
             new TransactionDto()
             {
                Id = 1,
                AccountId = 1,
                Date=new DateTime(2022,05,05),
                Amount = -10,
                Currency=Currency.EUR
             },
            new TransactionDto()
            {
                Id = 2,
                AccountId = 2,
                Date=new DateTime(2022,05,05),
                Amount = 20,
                Currency=Currency.RUB
            }
        };
        _calculationService.Setup(c => c.ConvertCurrency(transfers))
         .ReturnsAsync(covertModels);
        _transactionRepositoryMock.Setup(t => t.AddTransferTransactions(covertModels[0], covertModels[1]))
         .ReturnsAsync(expecteIds);
        _transactionRepositoryMock.Setup(t => t.GetBalanceByAccountId(transfers[0].AccountId))
        .ReturnsAsync(100);

        //when
        var actual = await _sut.AddTransfer(transfers);

        //then
        Assert.AreEqual(expecteIds, actual);
        _transactionRepositoryMock.Verify(t => t.AddTransferTransactions(covertModels[0], covertModels[1]), Times.Once);

    }

    [Test]
    public async Task AddTransfer_BalanceLessRequested_ThrowBadRequestException()
    {
        //given
        var realBalance = 1;
        var transfers = new List<TransactionDto>()
        {
            new TransactionDto()
            {
                Id = 1,
                AccountId = 1,
                Amount = 10,

                Currency=Currency.EUR
            },
            new TransactionDto()
            {
                Id = 2,
                AccountId = 2,
                Currency=Currency.RUB
            }
        };

        _transactionRepositoryMock.Setup(t => t.GetBalanceByAccountId(transfers[0].AccountId))
        .ReturnsAsync(realBalance);

        //when,then

        Assert.ThrowsAsync<BadRequestException>(() => _sut.AddTransfer(transfers));

        _transactionRepositoryMock.Verify(t => t.GetBalanceByAccountId(transfers[0].AccountId), Times.Once);
        _transactionRepositoryMock.Verify(t => t.AddTransaction(It.IsAny<TransactionDto>()), Times.Never);

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
        var transaction = new TransactionDto()
        {
            Id = 1,
            AccountId = 1,
            Date = new DateTime(2022, 05, 05),
            Amount = 10,
            TransactionType = TransactionType.Deposit,
            Currency = Currency.EUR
        };
        _transactionRepositoryMock.Setup(t => t.GetTransactionById(transaction.Id))
        .ReturnsAsync(transaction);

        //when
        var actual = await _sut.GetTransactionById(transaction.Id);

        //then
        Assert.AreEqual(actual.Id, transaction.Id);
        Assert.AreEqual(actual.AccountId, transaction.AccountId);
        Assert.AreEqual(actual.Date, transaction.Date);
        Assert.AreEqual(actual.Amount, transaction.Amount);
        Assert.AreEqual(actual.TransactionType, transaction.TransactionType);
        Assert.AreEqual(actual.Currency, transaction.Currency);
        _transactionRepositoryMock.Verify(t => t.GetTransactionById(transaction.Id), Times.Once);
    }

    [Test]
    public async Task GetTransactionById_TransactionIsNull_ThrowEntityNotFoundException()
    {
        //given
        var transactionId = 1l;
 
        //when, then
        Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetTransactionById(transactionId));

        _transactionRepositoryMock.Verify(t => t.GetTransactionById(transactionId), Times.Once);
    }

    [Test]
    public async Task GetTransactionsById_ValidRequestPassed_TransactionsReturned()
    {
        //given
        var transactions = new List<TransactionDto>()
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
        _transactionRepositoryMock.Setup(t => t.GetAllTransactionsByAccountId(transactions[0].AccountId))
        .ReturnsAsync(transactions);

        //when
        var actual = _sut.GetTransactionsByAccountId(transactions[0].AccountId);
        var actualWithdraw = actual.Result[transactions[3].Date];
        var actualDeposit = actual.Result[transactions[2].Date];
        var actualTransfer = actual.Result[transactions[1].Date];

        Assert.AreEqual(actualWithdraw[0], transactions[3]);
        Assert.AreEqual(actualDeposit[0], transactions[2]);
        Assert.AreEqual(actualTransfer[0], transactions[0]);
        Assert.AreEqual(actualTransfer[1], transactions[1]);
        _transactionRepositoryMock.Verify(t => t.GetAllTransactionsByAccountId(transactions[0].Id), Times.Once);
    }
}