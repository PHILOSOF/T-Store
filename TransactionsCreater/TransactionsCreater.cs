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
        
        var indexerTranfer = 0;
        var indexerDeposit = 0;
        foreach (var key in keys)
        {
            var transactionDeposit = new TransactionDtoToCsv();
            var sender = new TransactionDto();
            var transferRecipient= new TransactionDtoToCsv();
            var transfersResult = new List<TransactionDto>();
            var transferReverseTmp = new  List<TransactionDto>();

            var accountsClinet = accountsDictionary[key];                   //deposit
            var accountRubOrUsd = accountsClinet.Find(a => a.Currency ==  (int)Currency.RUB || a.Currency == (int)Currency.USD);
           
            if (accountRubOrUsd is not null)
            {
                indexerDeposit++;
                for ( int i=0; i < 2; i++ )
                {
                    transactionDeposit = await GetDeposit(transactionDeposit, accountRubOrUsd);
                    resultTransactions.Add(transactionDeposit);
                }

                switch (indexerDeposit)
                {
                    case 3:
                        var transactionWithdraw = await Getwithdraw((transactionDeposit.Amount/90), transactionDeposit); // withdraw usd or rub after reverse
                        transfersResult.Add(transactionWithdraw);

                        indexerDeposit = 0;
                        break;
                }
            }

            var accountsVip = accountsClinet.Where(a => a.Currency != accountRubOrUsd.Currency).ToList(); // transfer
            foreach (var account in accountsVip)
            {

                indexerTranfer++;
                switch (indexerTranfer)
                {
                    case 1:
                        transfersResult =await GetTransfers(transactionDeposit, account); // transfer to usd or rub
                        sender = transfersResult.Find(t => t.Currency == transactionDeposit.Currency);

                        var transferReverse = transfersResult[1] as TransactionDtoToCsv; // reverse transfer
                        transferReverse = new TransactionDtoToCsv(transferReverse);
                        var initialDeposit = transactionDeposit;
                        transferReverse.Amount = transferReverse.Amount / 50;
                        transfersResult.AddRange(await GetTransfers(transferReverse, accountRubOrUsd));

                        transferReverse.Amount = transferReverse.Amount / 50;
                        transfersResult.AddRange(await GetTransfers(transferReverse, accountRubOrUsd));

                        var transactionWithdraw = await Getwithdraw((transfersResult[3].Amount/50), (TransactionDtoToCsv)transfersResult[2]); // withdraw usd or rub after reverse
                        transfersResult.Add(transactionWithdraw);
                        break;
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        transfersResult = await GetTransfers((TransactionDtoToCsv)transfersResult[1], account);

                        transferReverse = transfersResult[1] as TransactionDtoToCsv;
                        transferReverse = new TransactionDtoToCsv(transferReverse);
                        initialDeposit = transactionDeposit;
                        transferReverse.Amount = transferReverse.Amount / 50;
                        transfersResult.AddRange(await GetTransfers(transferReverse, accountRubOrUsd));
                        break;
                }
                resultTransactions.AddRange(transfersResult.Cast<TransactionDtoToCsv>());
            }
            indexerTranfer = 0;

            var currentBalance = transactionDeposit.Amount + sender.Amount; // withdraw

            if (currentBalance > 50)
            {
                var transactionWithdraw = await Getwithdraw(currentBalance, transactionDeposit);
                resultTransactions.Add(transactionWithdraw);
            }        
        } 
        _transactionsToCsv.ConvertToCsv(resultTransactions.OrderBy(r => r.Date).ToList(), @"E:\sqlTestFiles\finalMax\FinalMax.txt");
    }

    private async Task<List<TransactionDto>> GetTransfers(TransactionDtoToCsv sender, Account recipient)
    {
        var transactionsTransferTmp = new List<TransactionDto>();
        var transferSender = new TransactionDtoToCsv (sender);
        var amoutFull = sender.Amount * CreatePercentForTransfer();
        transferSender.Amount = Math.Round(amoutFull, 4);
        transferSender.Date = CreateRandomDateStartingFromParameter(sender.Date);
        transferSender.TransactionType = TransactionType.Transfer;

        var transferRecipient = _mapper.Map<TransactionDtoToCsv>(recipient);
        transferRecipient.Date = transferSender.Date;
        transferRecipient.TransactionType = TransactionType.Transfer;

        transactionsTransferTmp.Add(transferSender);
        transactionsTransferTmp.Add(transferRecipient);

        var transactionsResult = await _calculationServices.ConvertCurrency(transactionsTransferTmp);
        transactionsResult[0].Amount = Math.Round(transactionsResult[0].Amount, 4);
        transactionsResult[1].Amount = Math.Round(transactionsResult[1].Amount, 4);
        return transactionsResult;
    }

    private async Task<TransactionDtoToCsv> GetDeposit(TransactionDtoToCsv deposit, Account accountRubOrUsd)
    {
        var random = new Random();
        deposit = _mapper.Map<TransactionDtoToCsv>(accountRubOrUsd);
        deposit.TransactionType = TransactionType.Deposit;
        deposit.Amount = random.Next(1000, 1000000);
        deposit.Date = CreateRandomDateStartingFromParameter(accountRubOrUsd.LeadRegistrationDate);
        return deposit;
    }

    private async Task<TransactionDtoToCsv> Getwithdraw( decimal currentBalance, TransactionDtoToCsv deposit )
    {
        var transactionWithdraw = new TransactionDtoToCsv(deposit);
        transactionWithdraw.TransactionType = TransactionType.Withdraw;
        transactionWithdraw.Amount = -currentBalance;
        transactionWithdraw.Date = CreateRandomDateStartingFromParameter(deposit.Date);
        return transactionWithdraw;
    }

    private DateTime CreateRandomDateStartingFromParameter(DateTime transactionTime)
    {
        Random gen = new Random();
        var randomDays = gen.Next(0, 15);
        var end = transactionTime.AddDays(randomDays);
        int range = (end - transactionTime).Days;
        transactionTime = transactionTime.Add(TimeSpan.FromMilliseconds(gen.Next(0, 1000)));



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