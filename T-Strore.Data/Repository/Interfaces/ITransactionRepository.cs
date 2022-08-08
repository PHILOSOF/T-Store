namespace T_Strore.Data.Repository;

public interface ITransactionRepository
{
    public Task<int> AddTransaction(TransactionDto transaction);
    public Task<List<int>> AddTransferTransactions(TransactionDto transactionSender, TransactionDto recipient);
    public Task<decimal?> GetBalanceByAccountId(int accountId);
    public Task<TransactionDto?> GetTransactionById(int id);
    public Task<List<TransactionDto>> GetAllTransactionsByAccountId(int accountId);
   
}