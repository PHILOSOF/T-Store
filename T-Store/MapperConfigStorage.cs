using AutoMapper;
using T_Store.Models;
using T_Strore.Data;

namespace T_Store;

public class MapperConfigStorage:Profile
{
    public MapperConfigStorage()
    {
        CreateMap<TransactionRequest, TransactionDto>();
        CreateMap<TransactionTransferRequest, TransactionDto>();
        CreateMap<TransactionDto, TransactionResponse>();

    }

}
