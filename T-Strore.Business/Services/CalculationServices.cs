using T_Strore.Data;

namespace T_Strore.Business.Services;

public class CalculationServices : ICalculationServices
{
    private readonly ICalculationServices _calculationService;

    public CalculationServices()
    {
        
    }

    public async Task<List<TransactionDto>> ConvertCurrency(List<TransactionDto> transferModels)
    {
        var currencyRates = await GetCurrencyRate();
        var senderCurrency = transferModels[0].Currency;
        var recipientCurrency = transferModels[1].Currency;
        

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
