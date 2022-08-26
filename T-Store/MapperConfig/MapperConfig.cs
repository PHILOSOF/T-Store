using AutoMapper;
using T_Store.Models;
using T_Strore.Data;

namespace T_Store.MapperConfig;
public class MapperConfig : Profile
{
    public MapperConfig()
    {

        CreateMap<TransactionRequest, TransactionDto>();

        CreateMap<TransactionDto, TransactionResponse>();
      
        CreateMap<TransactionTransferRequest, List<TransactionDto>>()
            .ConvertUsing<TransferRequestMapper>();

        CreateMap<Dictionary<DateTime, List<TransactionDto>>, List<TransactionResponse>>()
           .ConvertUsing<TransactionResponseMapper>();
    }
}


