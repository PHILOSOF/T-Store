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

        _logger.LogInformation("Business layer: Converting amount by currency");
        transferModels = GetConvertingAmountByCurrency(currencyRates, transferModels);
       
        transferModels[0].Amount *= -1;

        _logger.LogInformation("Business layer: Convert result returned");
        return transferModels;
    }

    private List<TransactionDto> GetConvertingAmountByCurrency(Dictionary<(string,string),decimal> currencyRates, List<TransactionDto> transferModels)
    {
        var senderCurrency = transferModels[0].Currency.ToString();
        var recipientCurrency = transferModels[1].Currency.ToString();
        var pairCurrencyWhithBase = currencyRates.Keys.ToList()
            .GroupBy(x => x.Item1)
            .First();

        if (pairCurrencyWhithBase.Key != senderCurrency && pairCurrencyWhithBase.Key != recipientCurrency)
        {
            transferModels[1].Amount = (transferModels[0].Amount /
            currencyRates[(pairCurrencyWhithBase.Key, senderCurrency)]) *
            currencyRates[(pairCurrencyWhithBase.Key, recipientCurrency)];
        }
        if (pairCurrencyWhithBase.Any(t => t.Item1 == senderCurrency && t.Item2 == recipientCurrency))
        {
            transferModels[1].Amount = transferModels[0].Amount * currencyRates[(pairCurrencyWhithBase.Key, recipientCurrency)];
        }
        if (pairCurrencyWhithBase.Any(t => t.Item1 == recipientCurrency && t.Item2 == senderCurrency))
        {
            transferModels[1].Amount = transferModels[0].Amount / currencyRates[(pairCurrencyWhithBase.Key, senderCurrency)];
        }
        return transferModels;
    }

    private async Task<Dictionary<(string, string), decimal>> GetCurrencyRate() // while we dont have service currency rate
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
            .ToDictionary(t => (Currency.USD.ToString(), t.ToString()),b=> (decimal)ratesList[(int)b-1]);

        return await Task.FromResult(ratesResult);
    }
}
