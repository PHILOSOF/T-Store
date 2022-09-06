using Microsoft.Extensions.Logging;
using T_Strore.Business.Models;

namespace T_Strore.Business.Services;

public class CalculationService : ICalculationService
{

    private readonly ILogger<CalculationService> _logger;

    public CalculationService( ILogger<CalculationService> logger)
    {
        _logger=logger;
    }

    public async Task<List<TransactionModel>> ConvertCurrency(List<TransactionModel> transferModels)
    {
        _logger.LogInformation("Business layer: Currency rate receiving");
        var currencyRates = await GetCurrencyRate();

        _logger.LogInformation("Business layer: Converting amount by currency");
        transferModels = GetConvertingAmountByCurrency(currencyRates, transferModels);
       
        transferModels[0].Amount *= -1;

        _logger.LogInformation("Business layer: Convert result returned");
        return transferModels;
    }


    private List<TransactionModel> GetConvertingAmountByCurrency(Dictionary<(string,string),decimal> currencyRates, List<TransactionModel> transferModels)
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
            //_logger.LogInformation($"Business layer: Exchange rate translation{currencyRates[(pairCurrencyWhithBase.Key, recipientCurrency)]}");
            transferModels[1].Amount = transferModels[0].Amount * currencyRates[(pairCurrencyWhithBase.Key, recipientCurrency)];
        }
        if (pairCurrencyWhithBase.Any(t => t.Item1 == recipientCurrency && t.Item2 == senderCurrency))
        {
            transferModels[1].Amount = transferModels[0].Amount / currencyRates[(pairCurrencyWhithBase.Key, senderCurrency)];
        }
        return transferModels;
    }

    private async Task<Dictionary<(string, string), decimal>> GetCurrencyRate()
    {
        var ratesDictionary = CurrencyRateModel.CurrencyRate;

        var withOutBase = ratesDictionary.ToDictionary(t => t.Key.Substring(3), t => t.Value);
        var baseCurrency = ratesDictionary.GroupBy(k => k.Key.Remove(3))
            .FirstOrDefault()
            .Key;

        var ratesResult = withOutBase
            .ToDictionary(t => (baseCurrency, t.Key.ToString()), b => b.Value);

        return await Task.FromResult(ratesResult);
    }
}
