using IncredibleBackendContracts.ExchangeModels;
using MassTransit;
using Microsoft.Extensions.Logging;
using T_Strore.Business.Models;

namespace T_Strore.Business.Consumers;

public class RateConsumer : IConsumer<CurrencyRate>
{

    private readonly ILogger<RateConsumer> _logger;

    public RateConsumer(ILogger<RateConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CurrencyRate> context)
    {
        var dictionaryConvert = new Dictionary<string, decimal>(context.Message.Rates);
        _logger.LogInformation($"RateConsumer: Save actual rates in model");
        CurrencyRateModel.CurrencyRates = dictionaryConvert;
    }
}