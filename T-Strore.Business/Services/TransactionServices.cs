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

        transaction.TransactionType = TransactionType.Deposit;

        return _transactionRepository.AddTransaction(transaction);
    }


    public int WithdrawDeposit(TransactionDto transaction)
    {
        CheckBalance(transaction);

        transaction.TransactionType = TransactionType.Withdraw;
        transaction.Amount = -transaction.Amount;
        return _transactionRepository.AddTransaction(transaction);
    }


    public List<int> AddTransfer(List<TransactionDto> transferModels)
    {
        var currencyRates = GetCurrencyRate();

        //CheckAccountByTypeCurrency(transactionRecipient);
        CheckBalance(transferModels[0]);

        //transactionSender.Currency = (Currency)_transactionRepository.GetCurrencyByAccountId(transactionSender.AccountId);

        if (transferModels[0].Currency != Currency.USD && transferModels[1].Currency != Currency.USD)
        {
            var currencyUsd = currencyRates[(Currency.USD, (Currency)transferModels[0].Currency)];
            var tmpTransferUsd = transferModels[0].Amount * currencyUsd;

            if (transferModels[0].Currency != Currency.USD)
                transferModels[1].Amount = tmpTransferUsd / currencyRates[(Currency.USD, (Currency)transferModels[1].Currency)];

            else
                transferModels[1].Amount = tmpTransferUsd * currencyRates[(Currency.USD, (Currency)transferModels[1].Currency)];
        }
        else
        {
            if (transferModels[0].Currency != Currency.USD)
                transferModels[1].Amount = transferModels[0].Amount / currencyRates[(Currency.USD, (Currency)transferModels[0].Currency)];

            else
                transferModels[1].Amount = transferModels[0].Amount * currencyRates[(Currency.USD, (Currency)transferModels[1].Currency)];
        }

        transferModels[0].TransactionType = TransactionType.Transfer;
        transferModels[1].TransactionType = TransactionType.Transfer;
        transferModels[0].Amount = -transferModels[0].Amount;

        return _transactionRepository.AddTransferTransactions(transferModels[0], transferModels[1]);
    }


    public decimal GetBalanceByAccountId(int accountId)
    {
        
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


        return _transactionRepository.GetAllTransactionsByAccountId(accountId);
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

}
