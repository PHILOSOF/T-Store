using T_Strore.Business.Services.Interfaces;
using T_Strore.Data;

namespace T_Strore.Business.Services;

public class CalculationService : ICalculationService
{

    public List<TransactionDto> ConvertCurrency(List<TransactionDto> transferModels)
    {
        var currencyRates = GetCurrencyRate();

        if (transferModels[0].Currency != Currency.USD && transferModels[1].Currency != Currency.USD)
        {
            var currencyUsd = currencyRates[(Currency.USD, (Currency)transferModels[0].Currency)];
            var tmpTransferUsd = transferModels[0].Amount * currencyUsd;

            if (transferModels[0].Currency != Currency.USD)
                transferModels[1].Amount = tmpTransferUsd / currencyRates[(Currency.USD, (Currency)transferModels[1].Currency)];

            else
                transferModels[1].Amount = tmpTransferUsd * currencyRates[(Currency.USD, (Currency)transferModels[1].Currency)];
        }
        else
        {
            if (transferModels[0].Currency != Currency.USD)
                transferModels[1].Amount = transferModels[0].Amount / currencyRates[(Currency.USD, (Currency)transferModels[0].Currency)];

            else
                transferModels[1].Amount = transferModels[0].Amount * currencyRates[(Currency.USD, (Currency)transferModels[1].Currency)];
        }

        transferModels[0].Amount = -transferModels[0].Amount;

        return transferModels;
    }

    private Dictionary<(Currency, Currency), decimal> GetCurrencyRate() =>
       Enum.GetValues(typeof(Currency))
       .Cast<Currency>()
              .ToDictionary(t => (Currency.USD, t), t => (decimal)t * 10);    // while we dont have service currency rate
}
