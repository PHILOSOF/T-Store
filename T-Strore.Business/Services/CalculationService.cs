using Microsoft.Extensions.Logging;
using NLog;
using T_Strore.Business.Exceptions;
using T_Strore.Business.Models;
using T_Strore.Business.Services.Interfaces;

namespace T_Strore.Business.Services;

public class CalculationService : ICalculationService
{

    private readonly ILogger<CalculationService> _logger;
    private readonly IRateService _rateService;
    private readonly object _locker = new object();

    public CalculationService( ILogger<CalculationService> logger, IRateService rateService)
    {
        _logger=logger;
        _rateService=rateService;
    }

    public async Task<List<TransactionModel>> ConvertCurrency(List<TransactionModel> transferModels)
    {
        var senderIndex = 0;
        var recipientIndex = 1;

        _logger.LogInformation("Business layer: Call GetCrossCurrencyRate method");
        var crossRate = _rateService.GetCrossCurrencyRate(transferModels[0].Currency.ToString(), transferModels[1].Currency.ToString());

        transferModels[recipientIndex].Amount = transferModels[senderIndex].Amount * crossRate;
        transferModels[senderIndex].Amount *= -1;

        _logger.LogInformation("Business layer: Transfers converted returned");
        return transferModels;
    }
}
