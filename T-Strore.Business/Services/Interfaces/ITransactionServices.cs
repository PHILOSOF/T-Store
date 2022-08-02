using T_Strore.Data;

namespace T_Strore.Business.Services;

public interface ITransactionServices
{
    public int AddDeposit(TransactionDto transaction);
    public List<int> AddTransfer(List<TransactionDto> transferModels);
    public decimal? GetBalanceByAccountId(int accountId);
    public TransactionDto? GetTransactionById(int id);
    public List<TransactionDto> GetTransactionsByAccountId(int accountId);
    public int WithdrawDeposit(TransactionDto transaction);
}