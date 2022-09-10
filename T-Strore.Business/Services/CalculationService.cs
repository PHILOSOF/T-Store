using Microsoft.Extensions.Logging;
using T_Strore.Business.Exceptions;
using T_Strore.Business.Models;
using T_Strore.Business.Services.Interfaces;

namespace T_Strore.Business.Services;

public class CalculationService : ICalculationService
{

    private readonly ILogger<CalculationService> _logger;
    private readonly IRateService _rateService;

    public CalculationService( ILogger<CalculationService> logger, IRateService rateService)
    {
        _logger=logger;
        _rateService=rateService;
    }

    public async Task<List<TransactionModel>> ConvertCurrency(List<TransactionModel> transferModels)
    {
        _logger.LogInformation("Business layer: Call GetCurrencyRate method");
        var currencyRates =  _rateService.GetRate();

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

}
