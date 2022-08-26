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
        _logger.LogInformation("Business layer: Request in data base for  add transaction");
        return await _transactionRepository.AddTransaction(transaction);
    }

    public async Task<long> Withdraw(TransactionDto transaction)
    {
        _logger.LogInformation("Business layer: Check balance");
        await CheckBalance(transaction);

        transaction.TransactionType = TransactionType.Withdraw;
        transaction.Amount = - transaction.Amount;

        _logger.LogInformation("Business layer: Request in data base for add withdraw");
        return await _transactionRepository.AddTransaction(transaction);
    }

    public async Task<List<long>> AddTransfer(List<TransactionDto> transfersModels)
    {
        _logger.LogInformation("Business layer: Check balance");
        await CheckBalance(transfersModels[0]);

       
        var transfersConvert = await _calculationService.ConvertCurrency(transfersModels);

        transfersConvert[0].TransactionType = TransactionType.Transfer;
        transfersConvert[1].TransactionType = TransactionType.Transfer;

        _logger.LogInformation("Business layer: Request in data base for add transfers");
        return await _transactionRepository.AddTransferTransactions(transfersConvert[0], transfersConvert[1]);
    }

    public async Task<decimal?> GetBalanceByAccountId(long accountId)
    {
        decimal emptyBalance = 0;

        _logger.LogInformation("Business layer: Request in data base for received balance");
        var balance = await _transactionRepository.GetBalanceByAccountId(accountId);

        if(balance is null)
        {
            _logger.LogInformation("Business layer: Balance returned in controller");
            return balance = emptyBalance;
        }

        _logger.LogInformation("Business layer: Balance returned in controller");
        return balance;
    }

    public async Task<TransactionDto?> GetTransactionById(long id)
    {
        _logger.LogInformation("Business layer: Request in data base for transaction receiving");
        var transaction = _transactionRepository.GetTransactionById(id);

        if (transaction.Result is null)
        {
            throw new EntityNotFoundException($"Transaction {id} not found");
        }

        _logger.LogInformation("Business layer: Transaction returned in controller");
        return await transaction;
    }

    public async Task<Dictionary<DateTime,List<TransactionDto>>> GetTransactionsByAccountId(long accountId)
    {
        _logger.LogInformation($"Business layer: Sending a request to database to get transactions by {accountId} id ");
        var transactions = await _transactionRepository.GetAllTransactionsByAccountId(accountId);

        _logger.LogInformation("Business layer: Add transactions in dictionary");
        var transactionsDictionary = transactions.GroupBy(t => t.Date).ToDictionary(d => d.Key, d => d.ToList());

        _logger.LogInformation("Business layer: Transactions returned to controller");
        return transactionsDictionary;
    }

    private async Task CheckBalance(TransactionDto transaction)
    {
        _logger.LogInformation("Business layer: request in data base for received balance");
        var balance = await _transactionRepository.GetBalanceByAccountId(transaction.AccountId);
        if (transaction.Amount > balance || balance is null || balance == 0)
        {
            throw new BadRequestException($"You have not a enough money on balance");
        }
    }
}
