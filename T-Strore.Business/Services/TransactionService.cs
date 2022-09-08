using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using T_Strore.Business.Exceptions;
using T_Strore.Business.Models;
using T_Strore.Business.Producers;
using T_Strore.Data.Repository;
using IncredibleBackendContracts.Enums;
using T_Strore.Data;

namespace T_Strore.Business.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICalculationService _calculationService;
    private readonly ILogger<TransactionService> _logger;
    private readonly IMapper _mapper;
    private readonly ITransactionProducer _transactionProducer;
    public TransactionService(ITransactionRepository transactionRepository, ICalculationService calculationService,
        IMapper mapper, ILogger<TransactionService> logger, ITransactionProducer transactionProducer)
    {
        _transactionRepository = transactionRepository;
        _calculationService = calculationService;
        _mapper = mapper;
        _logger = logger;
        _transactionProducer = transactionProducer;
    }

    public async Task<long> AddDeposit(TransactionModel transaction)
    {
        transaction.TransactionType = TransactionType.Deposit;

        _logger.LogInformation("Business layer: Query to data base for add transaction");
        var transactionIdResult = await _transactionRepository.AddTransaction(_mapper.Map<TransactionDto>(transaction));

        _logger.LogInformation($"Business layer: Call NotifyTransaction method for transaction id {transactionIdResult}");
        await _transactionProducer.NotifyTransaction(await GetTransactionById(transactionIdResult));

        return transactionIdResult;
    }

    public async Task<long> Withdraw(TransactionModel transaction)
    {
        _logger.LogInformation($"Business layer: Check balance by accoint id {transaction.AccountId}");
        await CheckBalance(transaction);

        transaction.TransactionType = TransactionType.Withdraw;
        transaction.Amount *= -1;

        _logger.LogInformation("Business layer: Query to data base for add withdraw");
        var transactionIdResult = await _transactionRepository.AddTransaction(_mapper.Map<TransactionDto>(transaction));

        _logger.LogInformation($"Business layer: Call NotifyTransaction method for transaction id {transactionIdResult}");
        await _transactionProducer.NotifyTransaction(await GetTransactionById(transactionIdResult));

        return transactionIdResult;
    }

    public async Task<List<long>> AddTransfer(List<TransactionModel> transfersModels)
    {
        int senderIndex = 0;
        _logger.LogInformation($"Business layer: Check balance by account id {transfersModels[senderIndex].AccountId}");
        await CheckBalance(transfersModels[senderIndex]);
       
        var transfersConvert = await _calculationService.ConvertCurrency(transfersModels);
        
        _logger.LogInformation("Business layer: Query to data base for add transfers");
        return await _transactionRepository.AddTransferTransactions(_mapper.Map<List<TransactionModel>, List<TransactionDto>>(transfersConvert));
    }

    public async Task<decimal?> GetBalanceByAccountId(long accountId)
    {
        _logger.LogInformation("Business layer: Query in data base for received balance");
        var balance = await _transactionRepository.GetBalanceByAccountId(accountId);

        _logger.LogInformation($"Business layer: Balance by account id {accountId} returned to controller");
        return balance;
    }

    public async Task<TransactionModel?> GetTransactionById(long id)
    {
        _logger.LogInformation("Business layer: Query in data base for transaction receiving");
        var transaction = await _transactionRepository.GetTransactionById(id);

        if (transaction is null)
        {
            throw new EntityNotFoundException($"Transaction {id} not found");
        }
        
        _logger.LogInformation("Business layer: Transaction returned to controller");
        return _mapper.Map<TransactionModel>(transaction);
    }

    public async Task<Dictionary<DateTime,List<TransactionModel>>> GetTransactionsByAccountId(long accountId)
    {
        _logger.LogInformation($"Business layer: Query in database for transaction by accountId {accountId}");
        var transactions = await _transactionRepository.GetAllTransactionsByAccountId(accountId);

        _logger.LogInformation($"Business layer: Add transactions in dictionary");
        var transactionsDictionary = _mapper.Map<List<TransactionDto>, List<TransactionModel>>(transactions)
            .GroupBy(t => t.Date)
            .ToDictionary(date => date.Key, transactions => transactions
            .ToList());
        
        _logger.LogInformation("Business layer: Transactions returned to controller");
        return transactionsDictionary;
    }

    private async Task CheckBalance(TransactionModel transaction)
    {
        _logger.LogInformation("Business layer: Query in data base for received balance");
        var balance = await _transactionRepository.GetBalanceByAccountId(transaction.AccountId);
        if (transaction.Amount > balance)
        {
            throw new BalanceExceedException($"You have not a enough money on balance");
        }
    }
}
