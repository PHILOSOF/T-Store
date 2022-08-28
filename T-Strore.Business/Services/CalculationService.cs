using Microsoft.Extensions.Logging;
using T_Strore.Data;

namespace T_Strore.Business.Services;

public class CalculationService : ICalculationService
{
    private readonly ILogger<CalculationService> _logger;
    public CalculationService(ILogger<CalculationService> logger)
    {
        _logger=logger;
    }

    public async Task<List<TransactionDto>> ConvertCurrency(List<TransactionDto> transferModels)
    {
        _logger.LogInformation("Business layer: Currency rate receiving");
        var currencyRates = await GetCurrencyRate();
        var senderCurrency = transferModels[0].Currency;   // .ToString() => RUB
        var recipientCurrency = transferModels[1].Currency;   // .ToString() => EUR

        // TryFind('RUBEUR')   RUBUSD -> false
        // TryFind('EURRUB')   USDRUB -> true; rate = 1 \ rate

        // var base = currencyRates.First().Key.substring(0, 3) => JPY
        // rate1 = currencyRates["JPY""RUB"]
        // rate2 = currencyRates["JPY""EUR"]


        _logger.LogInformation("Business layer: Converting amount by currency");
        if (senderCurrency != Currency.USD && recipientCurrency != Currency.USD)
        {
                transferModels[1].Amount = (transferModels[0].Amount /
                currencyRates[(Currency.USD, senderCurrency)]) *
                currencyRates[(Currency.USD, recipientCurrency)];
        }
        else
        {
            if (transferModels[0].Currency != Currency.USD)
                transferModels[1].Amount = transferModels[0].Amount / currencyRates[(Currency.USD, senderCurrency)];

            else
                transferModels[1].Amount = transferModels[0].Amount * currencyRates[(Currency.USD, recipientCurrency)];
        }

        transferModels[0].Amount = -transferModels[0].Amount;

        _logger.LogInformation("Business layer: Convert result returned");
        return transferModels;
    }

    private async Task<Dictionary<(Currency, Currency), decimal>> GetCurrencyRate() // while we dont have service currency rate
    {
        var ratesList = new List<decimal>() 
        { 
            0.98m,  //EUR
            61.88m, //RUB
            1,      //USD
            134.61m,//JPY
            406.61m,//AMD
            1.92m,  //BGN
            115.02m //RSD
        };
       var ratesResult = Enum.GetValues(typeof(Currency))
            .Cast<Currency>()
            .ToDictionary(t => (Currency.USD, t),b=> (decimal)ratesList[(int)b-1]);

        return await Task.FromResult(ratesResult);
    }
}
