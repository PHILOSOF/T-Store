using Dapper;
using T_Strore.Data.Repository.Interfaces;

namespace T_Strore.Data;

public class TransactionRepository : BaseRepository, ITransactionRepository
{

    public int AddTransaction(TransactionDto transaction)
    {
        var id = ConString.QueryFirstOrDefault<int>(
                  TransactionStoredProcedure.Transaction_Insert,
                  param: new
                  {
                      transaction.AccountId,
                      transaction.TransactionType,
                      transaction.Amount,
                      transaction.Currency
                  },
                  commandType: System.Data.CommandType.StoredProcedure);

        return id;
    }


    public decimal GetBalanceByAccountId(int accountId)
    {
        var balance = ConString.QueryFirstOrDefault<decimal>(
                 TransactionStoredProcedure.Transaction_SelectBalanceByAccountId,
                 param: new { accountId },
                 commandType: System.Data.CommandType.StoredProcedure);

        return balance;
    }


    public TransactionDto? GetTransactionById(int id)
    {
        var transaction = ConString.QueryFirstOrDefault<TransactionDto>(
                 TransactionStoredProcedure.Transaction_SelectById,
                 param: new { id },
                 commandType: System.Data.CommandType.StoredProcedure);

        return transaction;
    }


    public List<int> AddTransferTransactions(TransactionDto transactionSender, TransactionDto recipient)
    {
        var id = ConString.Query<int>(
                  TransactionStoredProcedure.Transaction_InsertTransfer,
                  param: new
                  {
                      AccountIdSender = transactionSender.AccountId,
                      AccountIdRecipient = recipient.AccountId,
                      Amount = transactionSender.Amount,
                      AmountConverted = recipient.Amount,
                      CurrencySender = transactionSender.Currency,
                      CurrencyRecipient = recipient.Currency,
                  },
                  commandType: System.Data.CommandType.StoredProcedure).ToList();

        return id;
    }


    public List<TransactionDto> GetAllTransactionsByAccountId(int accountId)
    {
        var transfers = ConString.Query<TransactionDto>(
                  TransactionStoredProcedure.Transaction_GetAllTransactionsByAccountId,
                  param: new { accountId },
                  commandType: System.Data.CommandType.StoredProcedure).ToList();

        return transfers;
    }
}
