using T_Strore.Data;

namespace T_Strore.Business.Services;

public interface ITransactionServices
{
    int AddDeposit(TransactionDto transaction);
    List<int> AddTransfer(List<TransactionDto> transferModels);
    decimal GetBalanceByAccountId(int accountId);
    TransactionDto? GetTransactionById(int id);
    List<TransactionDto> GetTransactionsByAccountId(int accountId);
    int WithdrawDeposit(TransactionDto transaction);
}