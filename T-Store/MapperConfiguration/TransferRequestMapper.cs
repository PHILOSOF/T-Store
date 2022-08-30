using AutoMapper;
using T_Store.Models;
using T_Strore.Business.Models;
using T_Strore.Data;

namespace T_Store.MapperConfiguration;

public class TransferRequestMapper :ITypeConverter<TransactionTransferRequest, List<T_Strore.Business.Models.TransactionModel>>
{
    public  List<T_Strore.Business.Models.TransactionModel> Convert(TransactionTransferRequest source, List<T_Strore.Business.Models.TransactionModel> destination, ResolutionContext context)
    {

        destination = new List<T_Strore.Business.Models.TransactionModel>()
         {
            new T_Strore.Business.Models.TransactionModel()
            {
                AccountId = source.AccountId,
                Amount = source.Amount,
                Currency = source.Currency,
                TransactionType = TransactionType.Transfer

            },
            new T_Strore.Business.Models.TransactionModel()
            {
                AccountId = source.RecipientAccountId,
                Currency = source.RecipientCurrency,
                TransactionType = TransactionType.Transfer

            },
         };

        return context.Mapper.Map<List<T_Strore.Business.Models.TransactionModel>>(destination);
    }
}
