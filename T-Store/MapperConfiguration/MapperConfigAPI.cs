using AutoMapper;
using IncredibleBackendContracts.Requests;
using IncredibleBackendContracts.Responses;
using T_Strore.Business.Models;

namespace T_Store.MapperConfiguration;
public class MapperConfigAPI : Profile
{
    public MapperConfigAPI()
    {

        CreateMap<TransactionRequest, TransactionModel>();

        CreateMap<TransactionModel, TransactionResponse>();

        CreateMap<TransactionTransferRequest, List<TransactionModel>>()
            .ConvertUsing<TransferRequestMapper>();

        CreateMap<Dictionary<DateTime, List<TransactionModel>>, List<TransactionResponse>>()
           .ConvertUsing<TransactionResponseMapper>();
    }
}


