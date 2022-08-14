using Dapper;
using System.Data;

namespace T_Strore.Data.Repository;

public class TransactionRepositories : BaseRepositories, ITransactionRepository
{
    public TransactionRepositories(IDbConnection dbConnection) 
        : base(dbConnection)
    {
    }

    public async Task<long> AddTransaction(TransactionDto transaction) =>
         await Connection.QueryFirstOrDefaultAsync<long>(
                  TransactionStoredProcedure.Transaction_Insert,
                  param: new
                  {
                      transaction.AccountId,
                      transaction.TransactionType,
                      transaction.Amount,
                      transaction.Currency
                  },
                  commandType: CommandType.StoredProcedure);

    public async Task<decimal?> GetBalanceByAccountId(long accountId) =>
        await Connection.QueryFirstOrDefaultAsync<decimal?>(
                 TransactionStoredProcedure.Transaction_SelectBalanceByAccountId,
                 param: new { accountId },
                 commandType: CommandType.StoredProcedure);

    public async Task<TransactionDto?> GetTransactionById(long id) =>
        await Connection.QueryFirstOrDefaultAsync<TransactionDto>(
                 TransactionStoredProcedure.Transaction_SelectById,
                 param: new { id },
                 commandType: CommandType.StoredProcedure);

    public async Task<List<long>> AddTransferTransactions(TransactionDto transactionSender, TransactionDto recipient) =>
        (await Connection.QueryAsync<long>(
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
                  commandType: CommandType.StoredProcedure)).ToList();

    public async Task<List<TransactionDto>> GetAllTransactionsByAccountId(long accountId) =>
        (await Connection.QueryAsync<TransactionDto>(
                  TransactionStoredProcedure.Transaction_GetAllTransactionsByAccountId,
                  param: new { accountId },
                  commandType: CommandType.StoredProcedure)).ToList();

}
