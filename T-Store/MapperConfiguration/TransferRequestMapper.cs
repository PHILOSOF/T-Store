using AutoMapper;
using T_Store.Models;
using T_Strore.Business.Models;
using T_Strore.Data;

namespace T_Store.MapperConfiguration;

public class TransferRequestMapper :ITypeConverter<TransactionTransferRequest, List<TransactionModel>>
{
    public  List<TransactionModel> Convert(TransactionTransferRequest source, List<TransactionModel> destination, ResolutionContext context)
    {

        destination = new List<TransactionModel>()
         {
            new TransactionModel()
            {
                AccountId = source.AccountId,
                Amount = source.Amount,
                Currency = source.Currency,
                TransactionType = TransactionType.Transfer

            },
            new TransactionModel()
            {
                AccountId = source.RecipientAccountId,
                Currency = source.RecipientCurrency,
                TransactionType = TransactionType.Transfer

            },
         };

        return context.Mapper.Map<List<TransactionModel>>(destination);
    }
}
