using T_Strore.Business.Exceptions;
using T_Strore.Business.Services.Interfaces;
using T_Strore.Data;
using T_Strore.Data.Repository.Interfaces;

namespace T_Strore.Business.Services;

public class TransactionServices : ITransactionServices
{

    private readonly ITransactionRepository _transactionRepository;
    private readonly ICalculationService _calculationService;


    public TransactionServices(ITransactionRepository transactionRepository, ICalculationService calculationService)
    {
        _transactionRepository = transactionRepository;
        _calculationService = calculationService;
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
        transaction.Amount = - transaction.Amount;
        return _transactionRepository.AddTransaction(transaction);
    }


    public List<int> AddTransfer(List<TransactionDto> transfersModels)
    {
       
        CheckBalance(transfersModels[0]);
        var transfersConvert = _calculationService.ConvertCurrency(transfersModels);

        transfersConvert[0].TransactionType = TransactionType.Transfer;
        transfersConvert[1].TransactionType = TransactionType.Transfer;
        
        return _transactionRepository.AddTransferTransactions(transfersConvert[0], transfersConvert[1]);
    }

   

    public decimal? GetBalanceByAccountId(int accountId)
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


   


    private void CheckBalance(TransactionDto transaction)
    {
        var balance = _transactionRepository.GetBalanceByAccountId(transaction.AccountId);
        if (transaction.Amount > balance)
        {
            throw new BadRequestException($"You have not a enough money on balance");
        }
    }

}
