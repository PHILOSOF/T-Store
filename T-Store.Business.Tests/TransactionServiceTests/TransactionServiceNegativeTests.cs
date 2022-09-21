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
using T_Strore.Business.Producers;

namespace T_Store.Business.Tests.TransactionServiceTests;

public class TransactionServiceNegativeTests
{
    private TransactionService _sut;
    private Mock<ITransactionRepository> _transactionRepositoryMock;
    private Mock<ICalculationService> _calculationService;
    private Mock<ILogger<TransactionService>> _logger;
    private IMapper _mapper;
    private Mock<IProcessorForProducer> _producerMock;


    [SetUp]
    public void Setup()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MapperConfigBusiness>()));
        _logger = new Mock<ILogger<TransactionService>>();
        _transactionRepositoryMock = new Mock<ITransactionRepository>();
        _calculationService = new Mock<ICalculationService>();
        _producerMock = new Mock<IProcessorForProducer>();
        _sut = new TransactionService(_transactionRepositoryMock.Object, _calculationService.Object,
                                                        _mapper, _logger.Object, _producerMock.Object);
    }


    [Test]
    public async Task WithdrawDeposit_BalanceLessRequested_ThrowBalanceExceedException()
    {
        //given
        var realBalance = 100;
        var transactionWithdraw = new TransactionModel()
        {
            Id = 1,
            AccountId = 1,
            Amount = 1000,
            Currency = Currency.EUR
        };

        _transactionRepositoryMock.Setup(t => t.GetBalanceByAccountId(transactionWithdraw.Id))
         .ReturnsAsync(realBalance);

        //when,then
        Assert.ThrowsAsync<BalanceExceedException>(() => _sut.Withdraw(_mapper.Map<TransactionModel>(transactionWithdraw)));

        _transactionRepositoryMock.Verify(t => t.GetBalanceByAccountId(transactionWithdraw.Id), Times.Once);
        _transactionRepositoryMock.Verify(t => t.AddTransaction(It.IsAny<TransactionDto>()), Times.Never);
    }


    [Test]
    public async Task AddTransfer_BalanceLessRequested_ThrowBadRequestException()
    {
        //given
        var realBalance = 1;
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

        _transactionRepositoryMock.Setup(t => t.GetBalanceByAccountId(transfers[0].AccountId))
        .ReturnsAsync(realBalance);

        //when,then

        Assert.ThrowsAsync<BalanceExceedException>(() => _sut.AddTransfer(transfers));

        _transactionRepositoryMock.Verify(t => t.GetBalanceByAccountId(transfers[0].AccountId), Times.Once);
        _transactionRepositoryMock.Verify(t => t.AddTransaction(It.IsAny<TransactionDto>()), Times.Never);

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
}
