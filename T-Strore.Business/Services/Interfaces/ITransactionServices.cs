using T_Strore.Data;

namespace T_Strore.Business.Services;

public interface ITransactionServices
{
    public Task<int> AddDeposit(TransactionDto transaction);
    public Task<List<int>> AddTransfer(List<TransactionDto> transferModels);
    public Task<decimal?> GetBalanceByAccountId(int accountId);
    public Task<TransactionDto?> GetTransactionById(int id);
    public Task<Dictionary<DateTime,List<TransactionDto>>> GetTransactionsByAccountId(int accountId);
    public Task<int> WithdrawDeposit(TransactionDto transaction);
}