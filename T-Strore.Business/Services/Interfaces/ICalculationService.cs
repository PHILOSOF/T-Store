using T_Strore.Data;

namespace T_Strore.Business.Services;

public interface ICalculationService
{
   public Task<List<TransactionDto>> ConvertCurrency(List<TransactionDto> transferModels);
}