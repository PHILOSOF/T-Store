﻿using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;

namespace T_Strore.Data.Repository;

public class TransactionRepositories : BaseRepositories, ITransactionRepository
{
    private readonly ILogger<TransactionRepositories> _logger;

    public TransactionRepositories(IDbConnection dbConnection, ILogger<TransactionRepositories> logger)
        : base(dbConnection)
    {
        _logger = logger;
    }

    public async Task<long> AddTransaction(TransactionDto transaction)
    {
        _logger.LogInformation("Data layer: Сonnection to data base");
        var id = await Connection.QueryFirstOrDefaultAsync<long>(
                  TransactionStoredProcedure.Transaction_Insert,
                  param: new
                  {
                      transaction.AccountId,
                      transaction.TransactionType,
                      transaction.Amount,
                      transaction.Currency
                  },
                  commandType: CommandType.StoredProcedure);

        _logger.LogInformation("Data layer: Transaction added, id returned in business");
        return id;
    }
         

    public async Task<decimal?> GetBalanceByAccountId(long accountId)
    {
        _logger.LogInformation("Data layer: Сonnection to data base");
        var balance = await Connection.QueryFirstOrDefaultAsync<decimal?>(
                 TransactionStoredProcedure.Transaction_SelectBalanceByAccountId,
                 param: new { accountId },
                 commandType: CommandType.StoredProcedure);

        _logger.LogInformation("Data layer: Balance returned in business");
        return balance;
    }
        
    public async Task<TransactionDto?> GetTransactionById(long id)
    {
        _logger.LogInformation("Data layer: Сonnection to data base");
        var transaction = await Connection.QueryFirstOrDefaultAsync<TransactionDto>(
                 TransactionStoredProcedure.Transaction_SelectById,
                 param: new { id },
                 commandType: CommandType.StoredProcedure);

        _logger.LogInformation("Data layer: Transaction returned in business");
        return transaction;
    }
          
    public async Task<List<long>> AddTransferTransactions(TransactionDto transactionSender, TransactionDto recipient)
    {
        _logger.LogInformation("Data layer: Сonnection to data base");
        var transferIds= (await Connection.QueryAsync<long>(
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

        _logger.LogInformation("Data layer: Transfer added, ids returned in business");
        return transferIds;
    }
        
    public async Task<List<TransactionDto>> GetAllTransactionsByAccountId(long accountId)
    {
        _logger.LogInformation("Data layer: Сonnection to data base");
        var transactions = (await Connection.QueryAsync<TransactionDto>(
                  TransactionStoredProcedure.Transaction_GetAllTransactionsByAccountId,
                  param: new { accountId },
                  commandType: CommandType.StoredProcedure)).ToList();

        _logger.LogInformation("Data layer: Transactions returned in business");
        return transactions;
    }       
}
