namespace T_Strore.Data.Repository.Interfaces
{
    public interface ITransactionRepository
    {
        int AddTransaction(TransactionDTO transaction);
        List<int> AddTransferTransactions(TransactionDTO transactionSender, TransactionDTO recipient);
        decimal GetBalanceByAccountId(int accountId);
        TransactionDTO GetTransactionById(int id);
        List<TransactionDTO> GetTransactionsByAccountId(int accountId);
        List<TransactionDTO> GetTransfersByAccountId(int accountId);
    }
}