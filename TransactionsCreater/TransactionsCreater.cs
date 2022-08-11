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
    public async Task CreateFirsDepositRub()
    {
        
        _accountReader = new AccountReader();
        _transactionsToCsv = new TransactionsToCsv();
        _calculationServices = new CalculationServices();
        
        var resultTransactionList = new List<TransactionDtoToCsv>();

        var accountsDictionary = _accountReader.GetDictionaryOut(@"E:\sqlTestFiles\Crm_Account_Out_Test.csv");
        var keys = accountsDictionary.Keys.ToList();
        var random = new Random();
        var indexerWithdraw = 0;

        foreach (var key in keys)
        {
            indexerWithdraw++;
            var transactionDeposit = new TransactionDtoToCsv();
            var transferSender = new TransactionDtoToCsv();
            var transferRecipient= new TransactionDtoToCsv();
            var transactionWithdraw= new TransactionDtoToCsv();
            var transactionsTransferTmp = new List<TransactionDto>();

            var accountsClinet = accountsDictionary[key];
            var rubOrUsdAccount = accountsClinet.Find(a => a.Currency ==  (int)Currency.RUB || a.Currency == (int)Currency.USD);
           

            if (rubOrUsdAccount is not null)
            {
                
                transactionDeposit.AccountId = rubOrUsdAccount.Id;
                transactionDeposit.Currency = (Currency)rubOrUsdAccount.Currency;
                transactionDeposit.LeadId = rubOrUsdAccount.LeadId;
                transactionDeposit.TransactionType = TransactionType.Deposit;
                transactionDeposit.Amount = random.Next(1000, 1000000);
                transactionDeposit.Date = CreateDateForDeposit();

                resultTransactionList.Add(transactionDeposit);
            }

            var accountRecipientTransfer = accountsClinet.Find(a => a.Currency != (int)Currency.RUB || a.Currency != (int)Currency.USD); // exept rub / usd ??
            var accountSenderTransfer = resultTransactionList.Find(t => t.LeadId == accountRecipientTransfer.LeadId);

            if(accountSenderTransfer is not null && accountRecipientTransfer is not null)
            {
                transferSender.Amount = accountSenderTransfer.Amount * CreatePercentForTransfer();
                transferSender.AccountId = accountSenderTransfer.AccountId;
                transferSender.Currency = accountSenderTransfer.Currency;
                transferRecipient.AccountId = accountRecipientTransfer.Id;
                transferSender.Date = CreateDateStartFromDeposit(accountSenderTransfer.Date);
                transferRecipient.Date = transferSender.Date;
                transferRecipient.Currency= (Currency)accountRecipientTransfer.Currency;
                transferSender.TransactionType = TransactionType.Transfer;
                transferRecipient.TransactionType = TransactionType.Transfer;// ??? 

                transactionsTransferTmp.Add(transferSender);//??
                transactionsTransferTmp.Add(transferRecipient);      //??


                var a = await _calculationServices.ConvertCurrency(transactionsTransferTmp);


                resultTransactionList.Add(a[0] as TransactionDtoToCsv);
                resultTransactionList.Add(a[1] as TransactionDtoToCsv);
            }

            if(indexerWithdraw == 3)
            {
                var currentBalance = transactionDeposit.Amount + transferSender.Amount;

                if(currentBalance > 100)
                {

                    transactionWithdraw.AccountId = transactionDeposit.AccountId;
                    transactionWithdraw.Currency = transactionDeposit.Currency;
                    transactionWithdraw.TransactionType = TransactionType.Withdraw;
                    transactionWithdraw.Amount = -currentBalance;
                    transactionWithdraw.Date = CreateDateStartFromDeposit(transferSender.Date);

                    resultTransactionList.Add(transactionWithdraw);
                }
                indexerWithdraw = 0;
            }    


        }
        
        _transactionsToCsv.ConvertToCsv(resultTransactionList.OrderBy(r => r.Date).ToList(), @"E:\sqlTestFiles\Crm_Account_To_Test.csv");
    }

    public DateTime CreateDateForDeposit()
    {
        Random gen = new Random();
        DateTime start = new DateTime(1995, 1, 1);
        int range = (DateTime.Today - start).Days;

        return start.AddDays(gen.Next(range))
            .AddHours(gen.Next(0, 24))
            .AddMinutes(gen.Next(0, 60))
            .AddSeconds(gen.Next(0, 60));
    }

    public DateTime CreateDateStartFromDeposit(DateTime transactionTime)
    {
        Random gen = new Random();
        var randomDays = gen.Next(0, 6);
        DateTime start = transactionTime;
        DateTime start1 = transactionTime.AddDays(randomDays);
        int range = (start1 - start).Days;

        return start.AddDays(gen.Next(range))
            .AddHours(gen.Next(0, 24))
            .AddMinutes(gen.Next(0, 60))
            .AddSeconds(gen.Next(0, 60));
    }

    public decimal CreatePercentForTransfer()
    {
        Random gen = new Random();
        return gen.Next(50, 100) / 100m;
    }
}