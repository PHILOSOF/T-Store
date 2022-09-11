﻿using Microsoft.Extensions.Logging;
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
            var withOutBase = rates.ToDictionary(t => t.Key.Substring(3), t => t.Value);

            _logger.LogInformation("Business layer: Find base currency");

            RateModel.baseCurrency = rates.GroupBy(k => k.Key.Remove(3))
                .FirstOrDefault()!
                .Key;

            _logger.LogInformation($"Business layer: Creating result dictionary which base currency {RateModel.baseCurrency}");
            var ratesResult = withOutBase
                .ToDictionary(t => (RateModel.baseCurrency, t.Key.ToString()), b => b.Value);

            _logger.LogInformation("Business layer: Rates result returned");

            RateModel.currencyRates = new(ratesResult);
        }
    }

    public Dictionary<(string, string), decimal> GetRate()
    {
        lock(_locker)
        {
            return RateModel.currencyRates;
        }
    }


    public decimal GetCrossCurrencyRate(string currencyRecipient, string currencyVariable)
    {
        var result = 1m;

        if(currencyRecipient != currencyVariable)
        {
            var test = GetRate();
            var recipientRate = test[(RateModel.baseCurrency, currencyRecipient)];
            var variableRate = test[(RateModel.baseCurrency, currencyVariable)];
            result = variableRate / recipientRate;
        }
        return result;
    }
}
