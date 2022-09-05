using MassTransit;
using T_Strore.Business.Models;

namespace T_Store.Consumers;

public class RateConsumer : IConsumer<RateModel>
{
    private readonly ILogger<RateConsumer> _logger;

    public RateConsumer(ILogger<RateConsumer> logger)
    {
        _logger = logger;
    }

    public async  Task Consume(ConsumeContext<RateModel> context)
    {
        _logger.LogInformation($"RateConsumer: {context.Message.TestRate}");
        await Console.Out.WriteLineAsync(context.Message.ToString());
    }
}
