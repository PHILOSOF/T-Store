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
                        var transactionModel = new TransactionResponse()
                        {
                            Id = transactions[0].Id,
                            AccountId = transactions[0].AccountId,
                            Date = transactions[0].Date,
                            TransactionType = transactions[0].TransactionType,
                            Amount = transactions[0].Amount,
                            Currency = transactions[0].Currency,

                        };
                        destination.Add(transactionModel);
                        break;
                    }

                case 2:
                    {
                        var transfers = source[key];
                        var transferModel = new TransferResponse()
                        {

                            Id = transfers[0].Id,
                            AccountId = transfers[0].AccountId,
                            Date = transfers[0].Date,
                            TransactionType = transfers[0].TransactionType,
                            Amount = transfers[0].Amount,
                            Currency = transfers[0].Currency,
                            IdRecipient = transfers[1].Id,
                            AccountIdRecipient = transfers[1].AccountId,
                            AmountRecipient = transfers[1].Amount,
                            CurrencyRecipient = transfers[1].Currency

                        };
                        destination.Add(transferModel);
                        break;
                    }
            }


        }

        return destination;
    }
}

