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

    [Test]
    public void CreateFirsDepositRub()
    {
        _accountReader = new AccountReader();
        _transactionsToCsv = new TransactionsToCsv();

        var transactionList = new List<TransactionDtoToCsv>();
        
        var accountsDictionary = _accountReader.GetDictionaryOut(@"E:\sqlTestFiles\Testcvs.csv");
        var keys = accountsDictionary.Keys.ToList();
        var random = new Random();

        foreach(var key in keys)
        {
            var transactionDto = new TransactionDtoToCsv();
            var accountsClinet = accountsDictionary[key];
            var rubAccount = accountsClinet.Find(a => a.Currency == (int)Currency.RUB); // rub/ usd ???

            if (rubAccount is not null)
            {
                transactionDto.AccountId = rubAccount.Id;
                transactionDto.Currency = (Currency)rubAccount.Currency;
                transactionDto.TransactionType = TransactionType.Deposit;
                transactionDto.Amount = random.Next(1000, 1000000);
                transactionDto.Date = CreateDate();
    
                transactionList.Add(transactionDto);
            }
        }
        _transactionsToCsv.ConvertToCsv(transactionList, @"E:\sqlTestFiles\TestToFinal.csv");
    }

    public DateTime CreateDate()
    {
        Random gen = new Random();
        DateTime start = new DateTime(1995, 1, 1);
        int range = (DateTime.Today - start).Days;

        return start.AddDays(gen.Next(range))
            .AddHours(gen.Next(0, 24))
            .AddMinutes(gen.Next(0, 60))
            .AddSeconds(gen.Next(0, 60));
    }
}