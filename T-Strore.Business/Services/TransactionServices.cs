using T_Strore.Business.Exceptions;
using T_Strore.Business.Services.Interfaces;
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
        transaction.TransactionType = TransactionType.Withdraw;

        return _transactionRepository.AddTransaction(transaction);
    }


    public List<int> AddTransfer(TransactionDto transactionSender, TransactionDto transactionRecipient)
    {
   


        transactionSender.TransactionType=TransactionType.Transfer;
        transactionRecipient.TransactionType= TransactionType.Transfer;

        transactionRecipient.Amount = transactionSender.Amount * 100;// курс

        return _transactionRepository.AddTransferTransactions(transactionSender, transactionRecipient);
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
        return _transactionRepository.GetTransactionsByAccountId(accountId);
    }


    public List<TransactionDto> GetTransfersByAccountId(int accountId)
    {
        return _transactionRepository.GetTransfersByAccountId(accountId);
    }

}
