using Microsoft.Extensions.Logging;
using T_Strore.Business.Exceptions;
using T_Strore.Data;
using T_Strore.Data.Repository;

namespace T_Strore.Business.Services;

public class TransactionService : ITransactionServices
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICalculationServices _calculationService;
    private readonly ILogger<TransactionService> _logger;
    public TransactionService(ITransactionRepository transactionRepository, ICalculationServices calculationService, ILogger<TransactionService> logger)
    {
        _transactionRepository = transactionRepository;
        _calculationService = calculationService;
        _logger = logger;
    }

    public async Task<long> AddDeposit(TransactionDto transaction)
    {
        transaction.TransactionType = TransactionType.Deposit;
        _logger.LogInformation("Business layer: Request in data base for  add transaction");
        return await _transactionRepository.AddTransaction(transaction);
    }

    public async Task<long> Withdraw(TransactionDto transaction)
    {
        _logger.LogInformation("Business layer: Check balance");
        await CheckBalance(transaction);

        transaction.TransactionType = TransactionType.Withdraw;
        transaction.Amount *= -1;

        _logger.LogInformation("Business layer: Request in data base for add withdraw");
        return await _transactionRepository.AddTransaction(transaction);
    }

    public async Task<List<long>> AddTransfer(List<TransactionDto> transfersModels)
    {
        _logger.LogInformation("Business layer: Check balance");
        await CheckBalance(transfersModels[0]);
       
        var transfersConvert = await _calculationService.ConvertCurrency(transfersModels);

        // to automapper
        transfersConvert[0].TransactionType = TransactionType.Transfer;
        transfersConvert[1].TransactionType = TransactionType.Transfer;

        _logger.LogInformation("Business layer: Request in data base for add transfers");
        return await _transactionRepository.AddTransferTransactions(transfersConvert[0], transfersConvert[1]);
    }

    public async Task<decimal?> GetBalanceByAccountId(long accountId)
    {
        _logger.LogInformation("Business layer: Request in data base for received balance");
        var balance = await _transactionRepository.GetBalanceByAccountId(accountId);

        _logger.LogInformation("Business layer: Balance returned in controller");
        return balance;
    }

    public async Task<TransactionDto?> GetTransactionById(long id)
    {
        _logger.LogInformation("Business layer: Request in data base for transaction receiving");
        var transaction = await _transactionRepository.GetTransactionById(id);

        if (transaction is null)
        {
            throw new EntityNotFoundException($"Transaction {id} not found");
        }

        _logger.LogInformation("Business layer: Transaction returned in controller");
        return transaction;
    }

    public async Task<Dictionary<DateTime,List<TransactionDto>>> GetTransactionsByAccountId(long accountId)
    {
        _logger.LogInformation("Business layer: Request in data base for transaction by id receiving");
        var transactions = await _transactionRepository.GetAllTransactionsByAccountId(accountId);

        _logger.LogInformation("Business layer: Add transactions in dictionary");
        var transactionsDictionary = transactions.GroupBy(t => t.Date).ToDictionary(d => d.Key, d => d.ToList());

        _logger.LogInformation("Business layer: Transactions returned in controller");
        return transactionsDictionary;
    }

    private async Task CheckBalance(TransactionDto transaction)
    {
        _logger.LogInformation("Business layer: request in data base for received balance");
        var balance = await _transactionRepository.GetBalanceByAccountId(transaction.AccountId);
        if (transaction.Amount > balance)
        {
            throw new BalanceExceedException($"You have not a enough money on balance");
        }
    }
}
