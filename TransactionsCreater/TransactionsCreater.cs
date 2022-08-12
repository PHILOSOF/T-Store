using NUnit.Framework;
using T_Strore.Business.Services;
using T_Strore.Data;
using TransactionsCreater.HelperCsv;
using TransactionsCreater.Model;

namespace TransactionsCreater;

internal class Tests
{
    private AccountReader _accountReader;
    private CalculationServices _calculationServices;
    private TransactionsToCsv _transactionsToCsv;

    [Test]
    public async Task CreateFakeTransactionsForDb()
    {
        
        _accountReader = new AccountReader();
        _transactionsToCsv = new TransactionsToCsv();
        _calculationServices = new CalculationServices();
        
        var resultTransactions = new List<TransactionDtoToCsv>();

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
                transactionDeposit.Date = CreateRandomDate();

                resultTransactions.Add(transactionDeposit);
            }

            var accountRecipientTransfer = accountsClinet.Find(a =>
            (a.Currency != (int)Currency.RUB || a.Currency != (int)Currency.USD) ||  a.Currency != rubOrUsdAccount.Currency);

            var accountSenderTransfer = resultTransactions.Find(t => t.LeadId == accountRecipientTransfer.LeadId);

            if(accountSenderTransfer is not null && accountRecipientTransfer is not null)
            {
                transferSender = new(accountSenderTransfer);
                transferSender.Amount = accountSenderTransfer.Amount * CreatePercentForTransfer();
                transferSender.Date = CreateRandomDateStartingFromParameter(accountSenderTransfer.Date);
                transferSender.TransactionType = TransactionType.Transfer;
              
                transferRecipient.AccountId = accountRecipientTransfer.Id;
                transferRecipient.Currency= (Currency)accountRecipientTransfer.Currency;
                transferRecipient.Date = transferSender.Date;
                transferRecipient.TransactionType = TransactionType.Transfer;
        
                transactionsTransferTmp.Add(transferSender);
                transactionsTransferTmp.Add(transferRecipient);


                var transfersResult = await _calculationServices.ConvertCurrency(transactionsTransferTmp);


                resultTransactions.Add(transfersResult[0] as TransactionDtoToCsv);
                resultTransactions.Add(transfersResult[1] as TransactionDtoToCsv);
            }

            switch(indexerWithdraw)
            {
                case 3:
                    var currentBalance = transactionDeposit.Amount + transferSender.Amount;

                    if (currentBalance > 50)
                    {
                        var transactionWithdraw = new TransactionDtoToCsv(transactionDeposit);
                        transactionWithdraw.TransactionType = TransactionType.Withdraw;
                        transactionWithdraw.Amount = -currentBalance;
                        transactionWithdraw.Date = CreateRandomDateStartingFromParameter(transferSender.Date);

                        resultTransactions.Add(transactionWithdraw);
                    }
                    indexerWithdraw = 0;
                    break;
            }
        }
        
        _transactionsToCsv.ConvertToCsv(resultTransactions.OrderBy(r => r.Date).ToList(), @"E:\sqlTestFiles\Crm_Account_To_Test.csv");
    }

    private DateTime CreateRandomDate()
    {
        Random gen = new Random();
        DateTime start = new DateTime(1995, 1, 1);
        int range = (DateTime.Today - start).Days;

        return start.AddDays(gen.Next(range))
            .AddHours(gen.Next(0, 24))
            .AddMinutes(gen.Next(0, 60))
            .AddSeconds(gen.Next(0, 60));
    }

    private DateTime CreateRandomDateStartingFromParameter(DateTime transactionTime)
    {
        Random gen = new Random();
        var randomDays = gen.Next(0, 15);
        DateTime start = transactionTime;
        DateTime end = transactionTime.AddDays(randomDays);
        int range = (end - start).Days;

        return start.AddDays(gen.Next(range))
            .AddHours(gen.Next(0, 24))
            .AddMinutes(gen.Next(0, 60))
            .AddSeconds(gen.Next(0, 60));
    }

    private decimal CreatePercentForTransfer()
    {
        Random gen = new Random();
        return gen.Next(50, 100) / 100m;
    }

    
}