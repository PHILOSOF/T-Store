using T_Strore.Data;

namespace T_Strore.Business.Services.Interfaces;

public interface ITransactionServices
{
    int AddDeposit(TransactionDto transaction);
    List<int> AddTransfer(TransactionDto transactionSender, TransactionDto transactionRecipient);
    decimal GetBalanceByAccountId(int accountId);
    TransactionDto? GetTransactionById(int id);
    List<TransactionDto> GetTransactionsByAccountId(int accountId);
    List<TransactionDto> GetTransfersByAccountId(int accountId);
    int WithdrawDeposit(TransactionDto transaction);
}