using T_Strore.Data;

namespace T_Strore.Business.Services;

public interface ITransactionServices
{
    public Task<long> AddDeposit(TransactionDto transaction);
    public Task<List<long>> AddTransfer(List<TransactionDto> transferModels);
    public Task<decimal?> GetBalanceByAccountId(long accountId);
    public Task<TransactionDto?> GetTransactionById(long id);
    public Task<Dictionary<DateTime,List<TransactionDto>>> GetTransactionsByAccountId(long accountId);
    public Task<long> Withdraw(TransactionDto transaction);
}