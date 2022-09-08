using IncredibleBackendContracts.Enums;
using MassTransit;
using Microsoft.Extensions.Logging;
using T_Strore.Business.Consumers;
using T_Strore.Business.Models;

namespace T_Strore.Business.Producers;

public class TransactionProducer : ITransactionProducer
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<RateConsumer> _logger;

    public TransactionProducer(IPublishEndpoint publishEndpoint, ILogger<RateConsumer> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task NotifyTransaction(TransactionModel model)
    {

        if (model.Currency == Currency.RUB)
            model.ExchangeRateToTheRuble = 1;
        else
            model.ExchangeRateToTheRuble = CurrencyRateModel.CurrencyRates["USDRUB"];

        _logger.LogInformation($"Business layer: Transaction id {model.Id} published");

        await _publishEndpoint.Publish(model);
    }
}
