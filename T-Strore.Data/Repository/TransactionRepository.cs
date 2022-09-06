using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;

namespace T_Strore.Data.Repository;

public class TransactionRepository : BaseRepository, ITransactionRepository
{
    private readonly ILogger<TransactionRepository> _logger;

    public TransactionRepository(IDbConnection dbConnection, ILogger<TransactionRepository> logger)
        : base(dbConnection)
    {
        _logger = logger;
    }

    public async Task<long> AddTransaction(TransactionDto transaction)
    {
        _logger.LogInformation("Data layer: Connection to data base");
        var id = await _dbConnection.QueryFirstOrDefaultAsync<long>(
                  TransactionStoredProcedure.Transaction_Insert,
                  param: new
                  {
                      transaction.AccountId,
                      transaction.TransactionType,
                      transaction.Amount,
                      transaction.Currency
                  },
                  commandType: CommandType.StoredProcedure);

        _logger.LogInformation("Data layer: Transaction added, id returned to business");
        return id;
    }
         

    public async Task<decimal> GetBalanceByAccountId(long accountId)
    {
        _logger.LogInformation("Data layer: Connection to data base");
        var balance = await _dbConnection.QueryFirstOrDefaultAsync<decimal>(
                 TransactionStoredProcedure.Transaction_SelectBalanceByAccountId,
                 param: new { accountId },
                 commandType: CommandType.StoredProcedure);

        _logger.LogInformation("Data layer: Balance returned to business");
        return balance;
    }
        
    public async Task<TransactionDto?> GetTransactionById(long id)
    {
        _logger.LogInformation("Data layer: Connection to data base");
        var transaction = await _dbConnection.QueryFirstOrDefaultAsync<TransactionDto>(
                 TransactionStoredProcedure.Transaction_SelectById,
                 param: new { id },
                 commandType: CommandType.StoredProcedure);

        _logger.LogInformation("Data layer: Transaction returned to business");
        return transaction;
    }
          
    public async Task<List<long>> AddTransferTransactions(List<TransactionDto> transfer)
    {
        _logger.LogInformation("Data layer: Connection to data base");
        var transferIds= (await _dbConnection.QueryAsync<long>(
                  TransactionStoredProcedure.Transaction_InsertTransfer,
                  param: new
                  {
                      AccountIdSender = transfer[0].AccountId,
                      AccountIdRecipient = transfer[1].AccountId,
                      Amount = transfer[0].Amount,
                      AmountConverted = transfer[1].Amount,
                      CurrencySender = transfer[0].Currency,
                      CurrencyRecipient = transfer[1].Currency,
                  },
                  commandType: CommandType.StoredProcedure)).ToList();

        _logger.LogInformation("Data layer: Transfer added, ids returned to business");
        return transferIds;
    }
        
    public async Task<List<TransactionDto>> GetAllTransactionsByAccountId(long accountId)
    {
        _logger.LogInformation("Data layer: Connection to data base");
        var transactions = (await _dbConnection.QueryAsync<TransactionDto>(
                  TransactionStoredProcedure.Transaction_GetAllTransactionsByAccountId,
                  param: new { accountId },
                  commandType: CommandType.StoredProcedure)).ToList();

        _logger.LogInformation("Data layer: Transactions returned to business");
        return transactions;
    }       
}
