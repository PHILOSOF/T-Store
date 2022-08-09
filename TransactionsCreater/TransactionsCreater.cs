using NUnit.Framework;
using T_Strore.Business.Services;
using T_Strore.Data;
using TransactionsCreater.HelperCsv;
using TransactionsCreater.Model;

namespace TransactionsCreater;

public class Tests
{
    private AccountReader _accountReader;
    private CalculationServices _calculationServices;
    private TransactionsToCsv _transactionsToCsv;

    [SetUp]
    public void Setup()
    {
        _accountReader = new AccountReader();
        _transactionsToCsv = new TransactionsToCsv();
    }


    [Test]
    public void CreateFirsDepositRub()
    {
        var transactionList = new List<TransactionDtoToCsv>();
        
        var accountsDictionary = _accountReader.GetAccounts("E:\\sqlTestFiles\\Testcvs.csv");
        var keys = accountsDictionary.Keys.ToList();
        var random = new Random();

        foreach(var key in keys)
        {
            var transactionDto = new TransactionDtoToCsv();
            var accountsClinet = accountsDictionary[key];
            var rubAccount = accountsClinet.Find(a => a.Currency =="2");

            if(rubAccount is not null)
            {
                transactionDto.AccountId = Int32.Parse(rubAccount.Id);
                transactionDto.Currency = (Currency)Int32.Parse(rubAccount.Currency);
                transactionDto.TransactionType = TransactionType.Deposit;
                transactionDto.Amount = random.Next(1000, 1000000);
                transactionDto.Date = CreateDate();

                

                transactionList.Add(transactionDto);
            }
        }


        _transactionsToCsv.GoToCsv(transactionList);

        Assert.NotNull(TransactionType.Deposit);
    }


    public DateTime CreateDate()
    {
        Random gen = new Random();
        DateTime start = new DateTime(1995, 1, 1,18,42,0);
        int range = (DateTime.Today - start).Days;

        return start.AddDays(gen.Next(range));
    }
}