using AutoMapper;
using IncredibleBackendContracts.Events;
using T_Strore.Business.Models;
using T_Strore.Data;

namespace T_Strore.Business.MapperConfiguration;

public class MapperConfigBusiness : Profile
{
    public MapperConfigBusiness()
    {
        CreateMap<TransactionModel, TransactionDto>().ReverseMap();
        CreateMap<TransactionModel, TransactionCreatedEvent>();

        CreateMap<(TransactionModel, TransactionModel), TransferTransactionCreatedEvent>()
            .ForMember(t => t.Id, act => act.MapFrom(l => l.Item1.Id))
            .ForMember(t => t.AccountId, act => act.MapFrom(l => l.Item1.AccountId))
            .ForMember(t => t.Date, act => act.MapFrom(l => l.Item1.Date))
            .ForMember(t => t.TransactionType, act => act.MapFrom(l => l.Item1.TransactionType))
            .ForMember(t => t.Amount, act => act.MapFrom(l => l.Item1.Amount))
            .ForMember(t => t.Currency, act => act.MapFrom(l => l.Item1.Currency))
            .ForMember(t => t.RecipientId, act => act.MapFrom(l => l.Item2.Id))
            .ForMember(t => t.RecipientAccountId, act => act.MapFrom(l => l.Item2.AccountId))
            .ForMember(t => t.RecipientAmount, act => act.MapFrom(l => l.Item2.Amount))
            .ForMember(t => t.RecipientCurrency, act => act.MapFrom(l => l.Item2.Currency));
    }
}
