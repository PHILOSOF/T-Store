using Microsoft.Extensions.Logging;
using T_Strore.Business.Exceptions;
using T_Strore.Business.Models;
using T_Strore.Business.Services.Interfaces;

namespace T_Strore.Business.Services;

public class RateService : IRateService
{
    private readonly ILogger<RateService> _logger;
    private readonly object _locker = new object();

    public RateService(ILogger<RateService> logger) =>_logger = logger;

    public void SaveCurrencyRate(Dictionary<string, decimal> rates)
    {
        lock(_locker)
        {
            if (rates is null)
            {
                throw new ServiceUnavailable($"Rates is epmty");
            }

            _logger.LogInformation("Business layer: Convert to the dictionary currency rates wihtout base currency");
            RateModel.currencyRates = rates.ToDictionary(t => t.Key.Substring(3), t => t.Value);

            _logger.LogInformation("Business layer: Find base currency");

            RateModel.baseCurrency = rates.GroupBy(k => k.Key.Remove(3))
                .FirstOrDefault()!
                .Key;
        }
    }

    public decimal GetCrossCurrencyRate(string currencyFirst, string currencySecond)
    {
        var result = 1m;

        lock(_locker)
        {
            if (currencyFirst != currencySecond)
            {
                var rates = GetRate();
                result = rates[currencySecond] / rates[currencyFirst];
            }
            return result;
        }
    }

    public Dictionary<string, decimal> GetRate()
    {
        lock(_locker)
        {
            return RateModel.currencyRates;
        }
    }
}
