using AutoMapper;
using T_Store.Models;
using T_Store.Models.Responses;
using T_Strore.Data;

namespace T_Store.MapperConfig;

public class TransferRespinseMapper : ITypeConverter<Dictionary<DateTime, List<TransactionDto>>, List<TransactionResponse>>
{
    public List<TransactionResponse> Convert(Dictionary<DateTime, List<TransactionDto>> source, List<TransactionResponse> destination, ResolutionContext context)
    {
        destination = new();
        foreach (var trans in source)
        {
            if (trans.Value.Count == 1)
            {
                var key = trans.Key;
                var tmp = source[trans.Key];
                var transresp = new TransactionResponse()
                {
                    Id = tmp[0].Id,
                    AccountId = tmp[0].AccountId,
                    Date = tmp[0].Date,
                    TransactionType = trans.Value[0].TransactionType,
                    Amount = trans.Value[0].Amount,
                    Currency = trans.Value[0].Currency,

                };
                destination.Add(transresp);
            }
            else
            {
                var key = trans.Key;
                var tmp = source[trans.Key];
                var transresp1 = new TransferResponse()
                {

                    Id = tmp[0].Id,
                    AccountId = tmp[0].AccountId,
                    Date=tmp[0].Date,
                    TransactionType=trans.Value[0].TransactionType,
                    Amount=trans.Value[0].Amount,
                    Currency=trans.Value[0].Currency,
                    IdRecipient = tmp[1].Id,
                    AccountIdRecipient = tmp[1].AccountId,
                    AmountRecipient = tmp[1].Amount,
                    CurrencyRecipient = tmp[1].Currency

                };
                destination.Add(transresp1);
            }


        }

        return destination;
    }
}

