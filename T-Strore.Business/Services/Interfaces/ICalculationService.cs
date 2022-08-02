using T_Strore.Data;

namespace T_Strore.Business.Services.Interfaces
{
    public interface ICalculationService
    {
       public List<TransactionDto> ConvertCurrency(List<TransactionDto> transferModels);
    }
}