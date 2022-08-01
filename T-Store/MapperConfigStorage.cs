using AutoMapper;
using T_Store.Models;
using T_Strore.Data;

namespace T_Store;

public class MapperConfigStorage:Profile
{
    public  MapperConfigStorage()
    {

        CreateMap<TransactionRequest, TransactionDto>()
            .ForMember(t => t.AccountId, opt => opt.MapFrom(t => t.AccountId))
         .ForMember(t => t.Amount, opt => opt.MapFrom(t => t.Amount))
        .ForMember(t => t.Currency, opt => opt.MapFrom(t => t.Currency));
        


        CreateMap<TransactionDto, TransactionResponse>();



        CreateMap<TransactionTransferRequest, List<TransactionDto>>()
        .ForMember(t => new TransactionDto().AccountId , opt => opt.MapFrom(r => r.CurrencyRecipient));




    }

    private List<TransactionDto> GetTransfers(TransactionResponse responseModel)
    {
        var transactions = new List<TransactionDto>();
        return transactions;

    }
        
}
