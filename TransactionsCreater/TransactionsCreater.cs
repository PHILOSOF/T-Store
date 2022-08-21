using AutoMapper;
using NUnit.Framework;
using T_Strore.Business.Services;
using T_Strore.Data;
using TransactionsCreater.HelperCsv;
using TransactionsCreater.Model;

namespace TransactionsCreater;

public class TransactionsCreater
{
    private AccountReader _accountReader;
    private CalculationServices _calculationServices;
    private TransactionsToCsv _transactionsToCsv;
    private  IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MapperForTransactionsCreater>()));
        _accountReader = new AccountReader();
        _transactionsToCsv = new TransactionsToCsv();
        _calculationServices = new CalculationServices();
    }

    [Test]
    public async Task CreateFakeTransactionsForDb()
    {


        var resultTransactions = new List<TransactionDtoToCsv>();
        
        var accountsDictionary = _accountReader.GetDictionaryOut(@"E:\SqlTestFiles\final\finalOut.txt");
        var keys = accountsDictionary.Keys.ToList();
        var random = new Random();
        var indexerTranfer = 0;

        foreach (var key in keys)
        {
            var transactionDeposit = new TransactionDtoToCsv();
            var sender = new TransactionDto();
            var transferRecipient= new TransactionDtoToCsv();
            var transfersResult = new List<TransactionDto>();

            var accountsClinet = accountsDictionary[key];                   //deposit
            var accountRubOrUsd = accountsClinet.Find(a => a.Currency ==  (int)Currency.RUB || a.Currency == (int)Currency.USD);
           
            if (accountRubOrUsd is not null)
            {
                transactionDeposit = _mapper.Map<TransactionDtoToCsv>(accountRubOrUsd);

                transactionDeposit.TransactionType = TransactionType.Deposit;
                transactionDeposit.Amount = random.Next(1000, 1000000);
                transactionDeposit.Date = CreateRandomDateStartingFromParameter(accountRubOrUsd.LeadRegistrationDate);

                resultTransactions.Add(transactionDeposit);
            }

            var accountsVip = accountsClinet.Where(a => a.Currency != accountRubOrUsd.Currency); // transfer
            foreach (var account in accountsVip)
            {
                
                indexerTranfer++;
                switch (indexerTranfer)
                {
                    case 1:
                        transfersResult = await GetTransfers(transactionDeposit, account);
                        sender = transfersResult.Find(t => t.Currency == transactionDeposit.Currency);
                        break;
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        transfersResult = await GetTransfers((TransactionDtoToCsv)transfersResult[1], account);//1-3/5
                        break;
                }
                resultTransactions.AddRange(transfersResult.Cast<TransactionDtoToCsv>());
            }
            indexerTranfer = 0;

            var currentBalance = transactionDeposit.Amount + sender.Amount; // withdraw

            if (currentBalance > 50)
            {
                var transactionWithdraw = new TransactionDtoToCsv(transactionDeposit);
                transactionWithdraw.TransactionType = TransactionType.Withdraw;
                transactionWithdraw.Amount = -currentBalance;
                transactionWithdraw.Date = CreateRandomDateStartingFromParameter(transactionDeposit.Date);

                resultTransactions.Add(transactionWithdraw);
            }        
        } 

        //if (indexerForCsvFile ???)
        //{
        //    var a = resultTransactions.Last();
        //    string path = @"E:\SqlTestFiles\final\TestManyFile\TEEEST.csv";
        //    using (FileStream fs = File.Create(path)) ;
        //}

        _transactionsToCsv.ConvertToCsv(resultTransactions.OrderBy(r => r.Date).ToList(), @"E:\sqlTestFiles\Crm_Account_To_Test.csv");
    }


    private async Task<List<TransactionDto>> GetTransfers(TransactionDtoToCsv sender, Account recipient)
    {
        var transactionsTransferTmp = new List<TransactionDto>();
        var transferSender = new TransactionDtoToCsv (sender);
        transferSender.Amount = sender.Amount * CreatePercentForTransfer();
        transferSender.Date = CreateRandomDateStartingFromParameter(sender.Date);
        transferSender.TransactionType = TransactionType.Transfer;

        var transferRecipient = _mapper.Map<TransactionDtoToCsv>(recipient);
        transferRecipient.Date = transferSender.Date;
        transferRecipient.TransactionType = TransactionType.Transfer;

        transactionsTransferTmp.Add(transferSender);
        transactionsTransferTmp.Add(transferRecipient);

        return await _calculationServices.ConvertCurrency(transactionsTransferTmp);
    }

    private DateTime CreateRandomDateStartingFromParameter(DateTime transactionTime)
    {
        Random gen = new Random();
        var randomDays = gen.Next(0, 15);
        var end = transactionTime.AddDays(randomDays);
        int range = (end - transactionTime).Days;

        return transactionTime.AddDays(gen.Next(range))
            .AddHours(gen.Next(0, 24))
            .AddMinutes(gen.Next(0, 60))
            .AddSeconds(gen.Next(0, 60));
    }

    private decimal CreatePercentForTransfer()
    {
        Random gen = new Random();
        return gen.Next(50, 100) / 100m;
    }

    public static void CreateEmptyFile(string filename)
    {
        File.Create(filename).Dispose();
    }
}