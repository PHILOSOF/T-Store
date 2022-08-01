using T_Strore.Business.Exceptions;
using T_Strore.Data;
using T_Strore.Data.Repository.Interfaces;

namespace T_Strore.Business.Services;

public class TransactionServices : ITransactionServices
{

    private readonly ITransactionRepository _transactionRepository;



    public TransactionServices(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }


    public int AddDeposit(TransactionDto transaction)
    {
        CheckAccountByTypeCurrency(transaction);

        transaction.TransactionType = TransactionType.Deposit;

        return _transactionRepository.AddTransaction(transaction);
    }


    public int WithdrawDeposit(TransactionDto transaction)
    {
        CheckAccountByTypeCurrency(transaction);
        CheckBalance(transaction);

        transaction.TransactionType = TransactionType.Withdraw;
        transaction.Amount = -transaction.Amount;
        return _transactionRepository.AddTransaction(transaction);
    }


    public List<int> AddTransfer(TransactionDto transactionSender, TransactionDto transactionRecipient)
    {
        var currencyRates = GetCurrencyRate();
        CheckAccount(transactionSender.AccountId);
        CheckAccountByTypeCurrency(transactionRecipient);
        CheckBalance(transactionSender);

        transactionSender.Currency = (Currency)_transactionRepository.GetCurrencyByAccountId(transactionSender.AccountId);

        if (transactionSender.Currency != Currency.USD && transactionRecipient.Currency != Currency.USD)
        {
            var currencyUsd = currencyRates[(Currency.USD, (Currency)transactionSender.Currency)];
            var tmpTransferUsd = transactionSender.Amount * currencyUsd;

            if(transactionSender.Currency != Currency.USD)
            transactionRecipient.Amount = tmpTransferUsd / currencyRates[(Currency.USD,(Currency)transactionRecipient.Currency)];

            else
                transactionRecipient.Amount = tmpTransferUsd * currencyRates[(Currency.USD, (Currency)transactionRecipient.Currency)];
        }
        else
        {
            if(transactionSender.Currency != Currency.USD)
            transactionRecipient.Amount = transactionSender.Amount / currencyRates[(Currency.USD,(Currency)transactionSender.Currency)];

            else
                transactionRecipient.Amount = transactionSender.Amount * currencyRates[(Currency.USD, (Currency)transactionRecipient.Currency)];
        }

        transactionSender.TransactionType = TransactionType.Transfer;
        transactionRecipient.TransactionType = TransactionType.Transfer;
        transactionSender.Amount = -transactionSender.Amount;

        return _transactionRepository.AddTransferTransactions(transactionSender, transactionRecipient);
    }


    public decimal GetBalanceByAccountId(int accountId)
    {
        CheckAccount(accountId);

        return _transactionRepository.GetBalanceByAccountId(accountId);
    }


    public TransactionDto? GetTransactionById(int id)
    {
        var transaction = _transactionRepository.GetTransactionById(id);
        if (transaction is null)
        {
            throw new EntityNotFoundException($"Transaction {id} not found");
        }

        return _transactionRepository.GetTransactionById(id);
    }


    public List<TransactionDto> GetTransactionsByAccountId(int accountId)
    {
        CheckAccount(accountId);

        return _transactionRepository.GetTransactionsByAccountId(accountId);
    }


    public List<TransactionDto> GetTransfersByAccountId(int accountId)
    {
        CheckAccount(accountId);

        return _transactionRepository.GetTransfersByAccountId(accountId);
    }


    private Dictionary<(Currency, Currency), decimal> GetCurrencyRate() =>
        Enum.GetValues(typeof(Currency))
        .Cast<Currency>()
               .ToDictionary(t => (Currency.USD, t), t => (decimal)t * 10);    // while we dont have service currency rate


    private void CheckBalance(TransactionDto transaction)
    {
        var balance = _transactionRepository.GetBalanceByAccountId(transaction.AccountId);
        if (transaction.Amount > balance)
        {
            throw new BadRequestException($"You have not a enough money on balance");
        }
    }


    private void CheckAccountByTypeCurrency(TransactionDto transaction)
    {
        var currency = _transactionRepository.GetCurrencyByAccountId(transaction.AccountId);

        if (currency != 0 && currency != ((int)transaction.Currency))
        {
            throw new BadRequestException($"Account currency does not match the transaction currency");
        }
    }


    private void CheckAccount(int accountId)
    {
        var cheked = _transactionRepository.CheckExistenceAccountId(accountId);

        if (!cheked)
        {
            throw new EntityNotFoundException($"Account {accountId} not found");
        }
    }
    
}
