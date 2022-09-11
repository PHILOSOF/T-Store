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


        //CreateMap<List<TransactionModel>, TransferTransactionCreatedEvent>()
        //    .ForMember(t => t.Id, act => act.MapFrom(l => l[0].Id))
        //    .ForMember(t => t.AccountId, act => act.MapFrom(l => l[0].AccountId))
        //    .ForMember(t => t.Date, act => act.MapFrom(l => l[0].Date))
        //    .ForMember(t => t.TransactionType, act => act.MapFrom(l => l[0].TransactionType))
        //    .ForMember(t => t.Amount, act => act.MapFrom(l => l[0].Amount))
        //    .ForMember(t => t.Currency, act => act.MapFrom(l => l[0].Currency))
        //    .ForMember(t => t.RecipientId, act => act.MapFrom(l => l[1].Id))
        //    .ForMember(t => t.RecipientAccountId, act => act.MapFrom(l => l[1].AccountId))
        //    .ForMember(t => t.RecipientAmount, act => act.MapFrom(l => l[1].Amount))
        //    .ForMember(t => t.RecipientCurrency, act => act.MapFrom(l => l[1].Currency));
    }
}
