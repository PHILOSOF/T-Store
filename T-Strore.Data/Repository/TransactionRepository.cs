using Dapper;
using System.Data;
using T_Strore.Data.Repository.Interfaces;

namespace T_Strore.Data;

public class TransactionRepository : BaseRepository, ITransactionRepository
{
    public TransactionRepository(IDbConnection dbConnection) 
        : base(dbConnection)
    {
    }

    public int AddTransaction(TransactionDto transaction)
    {
        var id = Connection.QueryFirstOrDefault<int>(
                  TransactionStoredProcedure.Transaction_Insert,
                  param: new
                  {
                      transaction.AccountId,
                      transaction.TransactionType,
                      transaction.Amount,
                      transaction.Currency
                  },
                  commandType: CommandType.StoredProcedure);

        return id;
    }


    public decimal? GetBalanceByAccountId(int accountId)
    {
        var balance = Connection.QueryFirstOrDefault<decimal?>(
                 TransactionStoredProcedure.Transaction_SelectBalanceByAccountId,
                 param: new { accountId },
                 commandType: CommandType.StoredProcedure);

        return balance;
    }


    public TransactionDto? GetTransactionById(int id)
    {
        var transaction = Connection.QueryFirstOrDefault<TransactionDto>(
                 TransactionStoredProcedure.Transaction_SelectById,
                 param: new { id },
                 commandType: CommandType.StoredProcedure);

        return transaction;
    }


    public List<int> AddTransferTransactions(TransactionDto transactionSender, TransactionDto recipient)
    {
        var id = Connection.Query<int>(
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
                  commandType: CommandType.StoredProcedure).ToList();

        return id;
    }


    public List<TransactionDto> GetAllTransactionsByAccountId(int accountId)
    {
        var transfers = Connection.Query<TransactionDto>(
                  TransactionStoredProcedure.Transaction_GetAllTransactionsByAccountId,
                  param: new { accountId },
                  commandType: CommandType.StoredProcedure).ToList();

        return transfers;
    }
}
