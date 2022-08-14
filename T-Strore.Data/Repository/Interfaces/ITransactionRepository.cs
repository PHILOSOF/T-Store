namespace T_Strore.Data.Repository;

public interface ITransactionRepository
{
    public Task<long> AddTransaction(TransactionDto transaction);
    public Task<List<long>> AddTransferTransactions(TransactionDto transactionSender, TransactionDto recipient);
    public Task<decimal?> GetBalanceByAccountId(long accountId);
    public Task<TransactionDto?> GetTransactionById(long id);
    public Task<List<TransactionDto>> GetAllTransactionsByAccountId(long accountId);
   
}