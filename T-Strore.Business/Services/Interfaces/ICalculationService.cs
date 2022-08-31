using T_Strore.Business.Models;

namespace T_Strore.Business.Services;

public interface ICalculationService
{
   public Task<List<TransactionModel>> ConvertCurrency(List<TransactionModel> transferModels);
}