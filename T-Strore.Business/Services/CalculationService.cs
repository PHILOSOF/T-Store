using Microsoft.Extensions.Logging;
using T_Strore.Business.Exceptions;
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
        _logger.LogInformation("Business layer: Call GetCurrencyRate method");
        var currencyRates = await GetCurrencyRate();

        _logger.LogInformation("Business layer: Call GetConvertingAmountByCurrency method");
        transferModels = GetConvertingAmountByCurrency(currencyRates, transferModels);
       
        transferModels[0].Amount *= -1;

        _logger.LogInformation("Business layer: Transfers converted returned");
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
            _logger.LogInformation($"Business layer: Transfer {senderCurrency}{recipientCurrency}, exchange rate: {pairCurrencyWhithBase.Key}{senderCurrency}:{currencyRates[(pairCurrencyWhithBase.Key, senderCurrency)]} and {pairCurrencyWhithBase.Key}{recipientCurrency}:{currencyRates[(pairCurrencyWhithBase.Key, recipientCurrency)]}");

            transferModels[1].Amount = (transferModels[0].Amount /
            currencyRates[(pairCurrencyWhithBase.Key, senderCurrency)]) *
            currencyRates[(pairCurrencyWhithBase.Key, recipientCurrency)];
        }
        if (pairCurrencyWhithBase.Any(t => t.Item1 == senderCurrency && t.Item2 == recipientCurrency))
        {
            _logger.LogInformation($"Business layer: Transfer{senderCurrency}{recipientCurrency}, exchange rate: {currencyRates[(pairCurrencyWhithBase.Key, recipientCurrency)]}");

            transferModels[1].Amount = transferModels[0].Amount * currencyRates[(pairCurrencyWhithBase.Key, recipientCurrency)];
        }
        if (pairCurrencyWhithBase.Any(t => t.Item1 == recipientCurrency && t.Item2 == senderCurrency))
        {
            _logger.LogInformation($"Business layer: Transfer{senderCurrency}{recipientCurrency}, exchange rate: {currencyRates[(pairCurrencyWhithBase.Key, recipientCurrency)]}");

            transferModels[1].Amount = transferModels[0].Amount / currencyRates[(pairCurrencyWhithBase.Key, senderCurrency)];
        }
        _logger.LogInformation($"Business layer: Total amount {transferModels[1].Amount} recipient id {transferModels[1].AccountId}");
        return transferModels;
    }

    private async Task<Dictionary<(string, string), decimal>> GetCurrencyRate()
    {
        var ratesDictionary = CurrencyRateModel.CurrencyRates;
        if (ratesDictionary is null)
        {
            throw new EntityNotFoundException($"Rates is epmty"); // while not working
        }

        _logger.LogInformation("Business layer: Convert to the dictionary currency rates wihtout base currency");
        var withOutBase = ratesDictionary.ToDictionary(t => t.Key.Substring(3), t => t.Value);

        _logger.LogInformation("Business layer: Find base currency");
        var baseCurrency = ratesDictionary.GroupBy(k => k.Key.Remove(3))
            .FirstOrDefault()
            .Key;

        _logger.LogInformation($"Business layer: Creating result dictionary which base currency {baseCurrency}");
        var ratesResult = withOutBase
            .ToDictionary(t => (baseCurrency, t.Key.ToString()), b => b.Value);

        _logger.LogInformation("Business layer: Rates result returned");
        return await Task.FromResult(ratesResult);
    }
}
