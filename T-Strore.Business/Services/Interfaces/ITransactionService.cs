using T_Strore.Business.Models;

namespace T_Strore.Business.Services;

public interface ITransactionService
{
    public Task<long> AddDeposit(TransactionModel transaction);
    public Task<List<long>> AddTransfer(List<TransactionModel> transferModels);
    public Task<decimal?> GetBalanceByAccountId(long accountId);
    public Task<TransactionModel?> GetTransactionById(long id);
    public Task<Dictionary<DateTime,List<TransactionModel>>> GetTransactionsByAccountId(long accountId);
    public Task<long> Withdraw(TransactionModel transaction);
}