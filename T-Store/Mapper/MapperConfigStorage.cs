using AutoMapper;
using T_Store.Models;
using T_Strore.Data;

namespace T_Store.Mapper;

public class MapperConfigStorage : Profile
{
    public MapperConfigStorage()
    {

        CreateMap<TransactionRequest, TransactionDto>()
            .ForMember(t => t.AccountId, opt => opt.MapFrom(t => t.AccountId))
         .ForMember(t => t.Amount, opt => opt.MapFrom(t => t.Amount))
        .ForMember(t => t.Currency, opt => opt.MapFrom(t => t.Currency));



        CreateMap<TransactionDto, TransactionResponse>();



        CreateMap<TransactionTransferRequest, List<TransactionDto>>().ConvertUsing<MapperHelper>();



    }



}

public class Custom : ITypeConverter<TransactionTransferRequest, List<TransactionDto>>
{


    public List<TransactionDto> Convert(TransactionTransferRequest source, List<TransactionDto> destination, ResolutionContext context)
    {



        destination = new List<TransactionDto>()
        {
            new TransactionDto()
            {
                AccountId = source.AccountId,
                Amount = source.Amount,
                Currency = source.Currency

            },
            new TransactionDto()
            {
                AccountId = source.AccountIdRecipient,
                Currency = source.CurrencyRecipient

            },
        };


        return destination;
    }
}
