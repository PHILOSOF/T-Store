using AutoMapper;
using T_Store.Models;
using T_Strore.Data;

namespace T_Store.Mapper;

public class MapperConfigStorage : Profile
{
    public MapperConfigStorage()
    {

        CreateMap<TransactionRequest, TransactionDto>();

        CreateMap<TransactionDto, TransactionResponse>();
            

        CreateMap<TransactionTransferRequest, List<TransactionDto>>()
            .ConvertUsing<MapperHelper>();
    }
}


