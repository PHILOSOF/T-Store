using AutoMapper;
using T_Store.Models;
using T_Store.Models.Responses;
using T_Strore.Data;

namespace T_Store.MapperConfig;

public class TransferResponseMapper : ITypeConverter<Dictionary<DateTime, List<TransactionDto>>, List<TransactionResponse>>
{
    public List<TransactionResponse> Convert(Dictionary<DateTime, List<TransactionDto>> source, List<TransactionResponse> destination, ResolutionContext context)
    {
        destination = new();
        var keys = source.Keys.ToList();
        
        foreach (var key in keys)
        {
            switch (source[key].Count)
            {
                case 1:
                    {
                        var transactions = source[key];
                        var transactionModel = context.Mapper.Map<TransactionResponse>(transactions[0]);
                        destination.Add(transactionModel);
                        break;
                    }

                case 2:
                    {
                        var transfers = source[key];

                        if (transfers[0].Amount < 0 && transfers[1].Amount > 0)
                        {
                            var transferModel = new TransferResponse()
                            {
                                Id = transfers[0].Id,
                                AccountId = transfers[0].AccountId,
                                Date = transfers[0].Date,
                                TransactionType = transfers[0].TransactionType,
                                Amount = transfers[0].Amount,
                                Currency = transfers[0].Currency,
                                RecipientId = transfers[1].Id,
                                RecipientAccountId = transfers[1].AccountId,
                                RecipientAmount = transfers[1].Amount,
                                RecipientCurrency = transfers[1].Currency
                            };
                            destination.Add(transferModel);
                        }
                        break;
                        
                    }
            }


        }

        return destination;
    }
}

