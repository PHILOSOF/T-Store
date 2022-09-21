using AutoMapper;
using IncredibleBackend.Messaging.Interfaces;
using IncredibleBackendContracts.Enums;
using IncredibleBackendContracts.Events;
using Moq;
using T_Strore.Business.MapperConfiguration;
using T_Strore.Business.Models;
using T_Strore.Business.Producers;
using T_Strore.Business.Services.Interfaces;

namespace T_Store.Business.Tests.MassTransitTests.ProducerTests;

public class ProcessorForProducerTests
{
    private ProcessorForProducer _sut;
    private Mock<IMessageProducer> _messageProducer;
    private IMapper _mapper;
    private Mock<IRateService> _rateServiceMock;

    [SetUp]
    public void Setup()
    {
        _messageProducer = new Mock<IMessageProducer>();
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MapperConfigBusiness>()));
        _rateServiceMock = new Mock<IRateService>();
        _sut = new ProcessorForProducer(_messageProducer.Object, _mapper, _rateServiceMock.Object);
    }

    [Test]
    public async Task NotifyTransaction_ValidRequestPassed_PublishTransactionModel()
    {
        //given
        var transaction = new TransactionModel()
        {
            Id = 1,
            TransactionType = TransactionType.Deposit,
            Date = new DateTime(2022, 05, 05),
            Amount = 10,
            AccountId = 1,
            Currency = Currency.EUR
        };

        //when
        await _sut.NotifyTransaction(transaction);

        //then
        _messageProducer.Verify(m => m.ProduceMessage(It.Is<TransactionCreatedEvent>(c =>
        c.Id == transaction.Id &&
        c.Currency == transaction.Currency &&
        c.TransactionType == transaction.TransactionType &&
        c.AccountId == transaction.AccountId &&
        c.Amount == transaction.Amount && 
        c.Date == transaction.Date &&
        c.Rate == 0), It.IsAny<String>()), Times.Once);
    }

    [Test]
    public async Task NotifyTransfer_ValidRequestPassed_PublishTransferModel()
    {
        //given
        var sender = new TransactionModel()
        {
            Id = 1,
            TransactionType = TransactionType.Transfer,
            Date = new DateTime(2022, 05, 05),
            Amount = 10,
            AccountId = 1,
            Currency = Currency.EUR

        };
        var recipient = new TransactionModel()
        {
            Id = 2,
            TransactionType = TransactionType.Transfer,
            Date = new DateTime(2022, 05, 05),
            Amount = 600,
            AccountId = 2,
            Currency = Currency.RUB

        };

        //when
        await _sut.NotifyTransfer(sender, recipient);

        //then
        _messageProducer.Verify(m => m.ProduceMessage(It.Is<TransferTransactionCreatedEvent>(c =>
        c.Id == sender.Id &&
        c.AccountId == sender.AccountId &&
        c.Date == sender.Date &&
        c.TransactionType == sender.TransactionType &&
        c.Amount == sender.Amount &&
        c.Currency == sender.Currency &&
        c.Rate == 0 && 
        c.RecipientId == recipient.Id &&
        c.RecipientAccountId == recipient.AccountId && 
        c.RecipientAmount == recipient.Amount &&
        c.RecipientCurrency == recipient.Currency), It.IsAny<String>()), Times.Once);
    }
}
