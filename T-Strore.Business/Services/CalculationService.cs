using Microsoft.Extensions.Logging;
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
        var senderIndex = 0;
        var recipientIndex = 1;

        _logger.LogInformation("Business layer: Call GetCurrencyRate method");
        var crossRate = _rateService.GetCurrencyRate(transferModels[senderIndex].Currency.ToString(), transferModels[recipientIndex].Currency.ToString());

        if (transferModels[0].Currency.ToString() == RateModel.BaseCurrency || transferModels[1].Currency.ToString() == RateModel.BaseCurrency)
        {
            _logger.LogInformation($"Business layer: Converting {transferModels[senderIndex].Currency} to {transferModels[recipientIndex].Currency} amount {transferModels[senderIndex].Amount}");
            transferModels[recipientIndex].Amount = transferModels[senderIndex].Currency.ToString() == RateModel.BaseCurrency ?
            transferModels[senderIndex].Amount * crossRate : transferModels[senderIndex].Amount / crossRate;
        }
        else
        {
            _logger.LogInformation($"Business layer: Converting {transferModels[0].Currency} to {transferModels[recipientIndex].Currency} amount {transferModels[senderIndex].Amount}");
            transferModels[recipientIndex].Amount =Math.Round((transferModels[senderIndex].Amount * crossRate),
                                                                       4, MidpointRounding.ToNegativeInfinity);
        }
        transferModels[senderIndex].Amount *= -1;

        _logger.LogInformation("Business layer: Transfers converted returned");
        return transferModels;
    }
}
