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

    public async Task<int> AddTransaction(TransactionDto transaction) =>
         await Connection.QueryFirstOrDefaultAsync<int>(
                  TransactionStoredProcedure.Transaction_Insert,
                  param: new
                  {
                      transaction.AccountId,
                      transaction.TransactionType,
                      transaction.Amount,
                      transaction.Currency
                  },
                  commandType: CommandType.StoredProcedure);

    public async Task<decimal?> GetBalanceByAccountId(int accountId) =>
        await Connection.QueryFirstOrDefaultAsync<decimal?>(
                 TransactionStoredProcedure.Transaction_SelectBalanceByAccountId,
                 param: new { accountId },
                 commandType: CommandType.StoredProcedure);

    public async Task<TransactionDto?> GetTransactionById(int id) =>
        await Connection.QueryFirstOrDefaultAsync<TransactionDto>(
                 TransactionStoredProcedure.Transaction_SelectById,
                 param: new { id },
                 commandType: CommandType.StoredProcedure);

    public async Task<List<int>> AddTransferTransactions(TransactionDto transactionSender, TransactionDto recipient) =>
        (await Connection.QueryAsync<int>(
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

    public async Task<List<TransactionDto>> GetAllTransactionsByAccountId(int accountId) =>
        (await Connection.QueryAsync<TransactionDto>(
                  TransactionStoredProcedure.Transaction_GetAllTransactionsByAccountId,
                  param: new { accountId },
                  commandType: CommandType.StoredProcedure)).ToList();

}
