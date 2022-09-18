using AutoMapper;
using IncredibleBackend.Messaging.Interfaces;
using IncredibleBackendContracts.Enums;
using IncredibleBackendContracts.Events;
using T_Strore.Business.Models;
using T_Strore.Business.Services.Interfaces;


namespace T_Strore.Business.Producers;

public class TransactionProducer : ITransactionProducer 
{
    private readonly IMessageProducer _messageProducer;
    private readonly IMapper _mapper;
    private readonly IRateService _rateService;
    public TransactionProducer(IMessageProducer messageProducer, IMapper mapper, IRateService rateService)
    {
        _messageProducer = messageProducer;
        _mapper = mapper;
        _rateService = rateService;
    }

    public async Task NotifyTransaction(TransactionModel model)
    {
        var modelForEvent = _mapper.Map<TransactionCreatedEvent>(model);
        var crossRateResult = _rateService.GetCurrencyRate(model.Currency.ToString(), Currency.RUB.ToString());
        modelForEvent.Rate = crossRateResult;
        var messageForLogger = $"Business layer: Transaction id {modelForEvent.Id} published";

        await _messageProducer.ProduceMessage(modelForEvent, messageForLogger);
    }

    public async Task NotifyTransfer(TransactionModel sender, TransactionModel recipient)
    {
        var modelForEvent = _mapper.Map<TransferTransactionCreatedEvent>((sender,recipient));
        var crossRateResult = _rateService.GetCurrencyRate(recipient.Currency.ToString(), Currency.RUB.ToString());
        modelForEvent.Rate = crossRateResult;
        var messageForLogger = $"Business layer: Transaction id {sender.Id},{recipient.Id} published";

        await _messageProducer.ProduceMessage(modelForEvent, messageForLogger);
    }
}
