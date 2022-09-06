using MassTransit;
using Microsoft.Extensions.Logging;
using T_Strore.Business.Models;

namespace T_Strore.Business.Consumers
{
    public class RateConsumer : IConsumer<RateModel>
    {

        private readonly ILogger<RateConsumer> _logger;

        public RateConsumer(ILogger<RateConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<RateModel> context)
        {
            _logger.LogInformation($"RateConsumer: Save actual rates in model");
            CurrencyRateModel.CurrencyRate=context.Message.TestRate;
        }
    }
}
