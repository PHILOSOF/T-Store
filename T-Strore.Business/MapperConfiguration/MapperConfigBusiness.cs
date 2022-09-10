using AutoMapper;
using T_Strore.Business.Models;
using T_Strore.Data;
using IncredibleBackendContracts.Responses;

namespace T_Strore.Business.MapperConfiguration;

public class MapperConfigBusiness : Profile
{
    public MapperConfigBusiness()
    {
        CreateMap<TransactionModel, TransactionDto>().ReverseMap();
        CreateMap<TransactionModel, TransactionCreatedEvent>();
    }
}
