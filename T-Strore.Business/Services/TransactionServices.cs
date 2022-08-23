using Microsoft.Extensions.Logging;
using T_Strore.Business.Exceptions;
using T_Strore.Data;
using T_Strore.Data.Repository;

namespace T_Strore.Business.Services;

public class TransactionServices : ITransactionServices
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICalculationServices _calculationService;
    private readonly ILogger<TransactionServices> _logger;
    public TransactionServices(ITransactionRepository transactionRepository, ICalculationServices calculationService, ILogger<TransactionServices> logger)
    {
        _transactionRepository = transactionRepository;
        _calculationService = calculationService;
        _logger = logger;
    }

    public async Task<long> AddDeposit(TransactionDto transaction)
    {

        transaction.TransactionType = TransactionType.Deposit;
        _logger.LogInformation("Add deposit, id returned");
        return await _transactionRepository.AddTransaction(transaction);
    }

    public async Task<long> Withdraw(TransactionDto transaction)
    {
        await CheckBalance(transaction);

        transaction.TransactionType = TransactionType.Withdraw;
        transaction.Amount = - transaction.Amount;
        _logger.LogInformation("Add withdraw, id returned");
        return await _transactionRepository.AddTransaction(transaction);
    }

    public async Task<List<long>> AddTransfer(List<TransactionDto> transfersModels)
    {
        _logger.LogInformation("Check balance");
        await CheckBalance(transfersModels[0]);

        _logger.LogInformation("Conver Currency");
        var transfersConvert = await _calculationService.ConvertCurrency(transfersModels);

        transfersConvert[0].TransactionType = TransactionType.Transfer;
        transfersConvert[1].TransactionType = TransactionType.Transfer;

        _logger.LogInformation("Add Transfer, ids returned");
        return await _transactionRepository.AddTransferTransactions(transfersConvert[0], transfersConvert[1]);
    }

    public async Task<decimal?> GetBalanceByAccountId(long accountId)
    {
        decimal emptyBalance = 0;

        _logger.LogInformation("Balance receiving");
        var balance = await _transactionRepository.GetBalanceByAccountId(accountId);

        if(balance is null)
        {
            return balance = emptyBalance;
        }

        _logger.LogInformation("Balance returned");
        return balance;
    }

    public async Task<TransactionDto?> GetTransactionById(long id)
    {
        _logger.LogInformation("Transaction receiving");
        var transaction = _transactionRepository.GetTransactionById(id);

        if (transaction.Result is null)
        {
            throw new EntityNotFoundException($"Transaction {id} not found");
        }

        _logger.LogInformation("Transaction returned");
        return await transaction;
    }

    public async Task<Dictionary<DateTime,List<TransactionDto>>> GetTransactionsByAccountId(long accountId)
    {
        _logger.LogInformation("Transactions receiving");
        var transactions = await _transactionRepository.GetAllTransactionsByAccountId(accountId);

        _logger.LogInformation("Transactions dictionary receiving");
        var transactionsDictionary = transactions.GroupBy(t => t.Date).ToDictionary(d => d.Key, d => d.ToList());

        _logger.LogInformation("Transactions returned");
        return transactionsDictionary;
    }

    private async Task CheckBalance(TransactionDto transaction)
    {
        _logger.LogInformation("Balance receiving");
        var balance = await _transactionRepository.GetBalanceByAccountId(transaction.AccountId);
        if (transaction.Amount > balance || balance is null || balance == 0)
        {
            throw new BadRequestException($"You have not a enough money on balance");
        }
    }
}
