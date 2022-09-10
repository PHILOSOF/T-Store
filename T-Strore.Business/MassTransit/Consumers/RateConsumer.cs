using IncredibleBackendContracts.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using T_Strore.Business.Services.Interfaces;

namespace T_Strore.Business.Consumers;

public class RateConsumer : IConsumer<NewRatesEvent >
{

    private readonly ILogger<RateConsumer> _logger;
    private readonly IRateService _rateService;

    public RateConsumer(ILogger<RateConsumer> logger, IRateService rateService)
    {
        _logger = logger;
        _rateService = rateService;
    }

    public async Task Consume(ConsumeContext<NewRatesEvent> context)
    {
        _logger.LogInformation($"RateConsumer: Save actual rates in model");
        _rateService.SaveCurrencyRate(context.Message.Rates);
    }
}