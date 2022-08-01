namespace T_Strore.Data.Repository.Interfaces;

public interface ITransactionRepository
{
    public int AddTransaction(TransactionDto transaction);
    public List<int> AddTransferTransactions(TransactionDto transactionSender, TransactionDto recipient);
    public decimal GetBalanceByAccountId(int accountId);
    public TransactionDto? GetTransactionById(int id);
    public List<TransactionDto> GetTransactionsByAccountId(int accountId);
    public List<TransactionDto> GetTransfersByAccountId(int accountId);
    public int GetCurrencyByAccountId(int accountId);
    public bool CheckExistenceAccountId(int accountId);
   
}