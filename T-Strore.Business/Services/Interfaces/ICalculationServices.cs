using T_Strore.Data;

namespace T_Strore.Business.Services;

public interface ICalculationServices
{
   public Task<List<TransactionDto>> ConvertCurrency(List<TransactionDto> transferModels);
}