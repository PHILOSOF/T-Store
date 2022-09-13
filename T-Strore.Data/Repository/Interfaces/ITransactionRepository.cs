namespace T_Strore.Data.Repository;

public interface ITransactionRepository
{
    public Task<long> AddTransaction(TransactionDto transaction);
    public Task<List<long>> AddTransferTransactions(List<TransactionDto> transfer);
    public Task<decimal> GetBalanceByAccountId(long accountId);
    public Task<TransactionDto?> GetTransactionById(long id);
    public Task<List<TransactionDto>> GetAllTransactionsByAccountId(long accountId);
    public Task<TransactionDto?> GetLastTransactionByAccountId(long accountId);
}