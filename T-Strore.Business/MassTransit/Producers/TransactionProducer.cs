using IncredibleBackendContracts.Enums;
using MassTransit;
using Microsoft.Extensions.Logging;
using T_Strore.Business.Models;
using IncredibleBackendContracts.Responses;
using AutoMapper;
using T_Strore.Business.Services;
using T_Strore.Business.Services.Interfaces;

namespace T_Strore.Business.Producers;

public class TransactionProducer : ITransactionProducer 
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<TransactionProducer> _logger;
    private readonly IMapper _mapper;
    private readonly IRateService _rateService;
    public TransactionProducer(IPublishEndpoint publishEndpoint, ILogger<TransactionProducer> logger, IMapper mapper, IRateService rateService)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
        _mapper = mapper;
        _rateService = rateService;
    }

    public async Task NotifyTransaction(TransactionModel model)
    {
        var modelForEvent = _mapper.Map<TransactionCreatedEvent>(model);
        var crossRateResult = _rateService.GetCurrencyRate(model.Currency.ToString(), Currency.RUB.ToString());
        modelForEvent.Rate = crossRateResult;

        _logger.LogInformation($"Business layer: Transaction id {modelForEvent.Id} published");
        await _publishEndpoint.Publish(modelForEvent);
    }

    public async Task NotifyTransfer(TransactionModel sender, TransactionModel recipient)
    {
        var modelForEvent = _mapper.Map<TransferTransactionCreatedEvent>((sender,recipient));
        var crossRateResult = _rateService.GetCurrencyRate(recipient.Currency.ToString(), Currency.RUB.ToString());
        modelForEvent.Rate = crossRateResult;
        _logger.LogInformation($"Business layer: Transaction id {sender.Id},{recipient.Id} published");
        await _publishEndpoint.Publish(modelForEvent);
    }
}
