using AutoMapper;
using T_Strore.Data;
using TransactionsCreater.Model;

namespace TransactionsCreater;

public class MapperForTransactionsCreater : Profile
{
    public MapperForTransactionsCreater()
    {
        CreateMap<Account, TransactionDtoToCsv>()
            .ForMember(x => x.AccountId, opt => opt.MapFrom(a => a.Id))
            .ForMember(x => x.Currency, opt => opt.MapFrom(a => (Currency)a.Currency));    
    }
}
