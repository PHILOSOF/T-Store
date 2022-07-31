using T_Strore.Business.Exceptions;
using T_Strore.Business.Services.Interfaces;
using T_Strore.Business.ServicesExtensions;
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
        var currency = _transactionRepository.GetCurrencyByAccountId(transaction.AccountId);
        if (currency != 0 && currency != ((int)transaction.Currency))
        {
            throw new BadRequestException($"Account currency does not match the transaction currency");
        }

        if (transaction.Amount <=0)
        {
            throw new BadRequestException($"Amount cannot be less than zero");
        }

        transaction.TransactionType = TransactionType.Deposit;

        return _transactionRepository.AddTransaction(transaction);
    }


    public int WithdrawDeposit(TransactionDto transaction)
    {
        var currency = _transactionRepository.GetCurrencyByAccountId(transaction.AccountId);
        if (currency != 0 && currency != ((int)transaction.Currency))
        {
            throw new BadRequestException($"Account currency does not match the transaction currency");
        }

        if (transaction.Amount <= 0)
        {
            
            throw new BadRequestException($"Amount cannot be less than zero");
        }
        transaction.TransactionType = TransactionType.Withdraw;

        return _transactionRepository.AddTransaction(transaction);
    }


    public List<int> AddTransfer(TransactionDto transactionSender, TransactionDto transactionRecipient)
    {
        //Dictionary<Currency, decimal> aDictionary = new()


        var сurrencyDictionary = typeof(Currency).EnumToDictionary();

        var currency = _transactionRepository.GetCurrencyByAccountId(transactionRecipient.AccountId);
        if (currency != 0 && currency != ((int)transactionRecipient.Currency))
        {
            throw new BadRequestException($"Account currency does not match the transaction currency");
        }
        Currency eb = (Currency)currency;
        //var curr = сurrencyDictionary[eb.ToString()];


        transactionSender.TransactionType = TransactionType.Transfer;
        transactionRecipient.TransactionType = TransactionType.Transfer;

        //transactionRecipient.Amount = transactionSender.Amount * Decimal.Parse(curr);// курс
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





    private void CheckAccount(int accountId)
    {
        var cheked = _transactionRepository.CheckExistenceAccountId(accountId);

        if (!cheked)
        {
            throw new EntityNotFoundException($"Account {accountId} not found");
        }
    }
    
}
