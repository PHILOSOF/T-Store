using T_Strore.Business.Exceptions;
using T_Strore.Data;
using T_Strore.Data.Repository;

namespace T_Strore.Business.Services;

public class TransactionServices : ITransactionServices
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICalculationServices _calculationService;

    public TransactionServices(ITransactionRepository transactionRepository, ICalculationServices calculationService)
    {
        _transactionRepository = transactionRepository;
        _calculationService = calculationService;
    }

    public async Task<long> AddDeposit(TransactionDto transaction)
    {

        transaction.TransactionType = TransactionType.Deposit;

        return await _transactionRepository.AddTransaction(transaction);
    }

    public async Task<long> Withdraw(TransactionDto transaction)
    {
        await CheckBalance(transaction);

        transaction.TransactionType = TransactionType.Withdraw;
        transaction.Amount = - transaction.Amount;
        return await _transactionRepository.AddTransaction(transaction);
    }

    public async Task<List<long>> AddTransfer(List<TransactionDto> transfersModels)
    {
        await CheckBalance(transfersModels[0]);

        var transfersConvert = await _calculationService.ConvertCurrency(transfersModels);

        transfersConvert[0].TransactionType = TransactionType.Transfer;
        transfersConvert[1].TransactionType = TransactionType.Transfer;
        
        return await _transactionRepository.AddTransferTransactions(transfersConvert[0], transfersConvert[1]);
    }

    public async Task<decimal?> GetBalanceByAccountId(long accountId)
    {
        decimal emptyBalance = 0;
        var balance = await _transactionRepository.GetBalanceByAccountId(accountId);

        if(balance is null)
        {
            return balance = emptyBalance;
        }

        return balance;
    }

    public async Task<TransactionDto?> GetTransactionById(long id)
    {
        var transaction = _transactionRepository.GetTransactionById(id);

        if (transaction.Result is null)
        {
            throw new EntityNotFoundException($"Transaction {id} not found");
        }

        return await transaction;
    }

    public async Task<Dictionary<DateTime,List<TransactionDto>>> GetTransactionsByAccountId(long accountId)
    {
        var transactions = await _transactionRepository.GetAllTransactionsByAccountId(accountId);
        var transactionsDictionary = transactions.GroupBy(t => t.Date).ToDictionary(d => d.Key, d => d.ToList());

        return transactionsDictionary;
    }

    private async Task CheckBalance(TransactionDto transaction)
    {
        var balance = await _transactionRepository.GetBalanceByAccountId(transaction.AccountId);
        if (transaction.Amount > balance || balance is null || balance == 0)
        {
            throw new BadRequestException($"You have not a enough money on balance");
        }
    }
}
