using IncredibleBackendContracts.Enums;
using MassTransit;
using Microsoft.Extensions.Logging;
using T_Strore.Business.Models;
using IncredibleBackendContracts.Responses;
using AutoMapper;

namespace T_Strore.Business.Producers;

public class TransactionProducer : ITransactionProducer 
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<TransactionProducer> _logger;
    private readonly IMapper _mapper;

    public TransactionProducer(IPublishEndpoint publishEndpoint, ILogger<TransactionProducer> logger, IMapper mapper)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task NotifyTransaction(TransactionModel model)
    {
        var a = GetTransaction(model);
        
        if (model.TransactionType != TransactionType.Transfer)
        {
            switch (model.Currency)
            {
                case Currency.RUB:
                    model.ExchangeRateToTheRuble = 1;
                    break;
                case Currency.USD:
                   // model.ExchangeRateToTheRuble = CurrencyRateModel.CurrencyRates["USDRUB"];
                    break;
            }  
        }

        _logger.LogInformation($"Business layer: Transaction id {model.Id} published");
        await _publishEndpoint.Publish(model);
    }

   
    private TransactionCreatedEvent GetTransaction(TransactionModel model)
    {
        var modelForEvent = _mapper.Map<TransactionCreatedEvent>(model);

        return modelForEvent;

    }
}
