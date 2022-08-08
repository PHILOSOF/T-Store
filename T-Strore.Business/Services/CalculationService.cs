using T_Strore.Data;

namespace T_Strore.Business.Services;

public class CalculationService : ICalculationService
{
    public async Task<List<TransactionDto>> ConvertCurrency(List<TransactionDto> transferModels)
    {
        var currencyRates = await GetCurrencyRate();
        var senderCurrency = transferModels[0].Currency;
        var recipientCurrency = transferModels[1].Currency;
        

        if (senderCurrency != Currency.USD && recipientCurrency != Currency.USD)
        {
                transferModels[1].Amount = transferModels[0].Amount *
                currencyRates[(Currency.USD, senderCurrency)] /
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

        return transferModels;
    }

    private async Task<Dictionary<(Currency, Currency), decimal>> GetCurrencyRate() // while we dont have service currency rate
    {
       var rates = Enum.GetValues(typeof(Currency))
            .Cast<Currency>()
            .ToDictionary(t => (Currency.USD, t), t => (decimal)t * 10);

        return await Task.FromResult(rates);
    }
}
