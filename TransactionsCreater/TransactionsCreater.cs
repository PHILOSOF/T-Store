using AutoMapper;
using NUnit.Framework;
using System.Data;
using System.Data.SqlClient;
using T_Strore.Business.Services;
using T_Strore.Data;
using TransactionsCreater.HelperCsv;
using TransactionsCreater.Model;

namespace TransactionsCreater;

public class TransactionsCreater
{
    private AccountReader _accountReader;
    private CalculationServices _calculationServices;
    private TransactionsCsvHelper _transactionsToCsv;
    private  IMapper _mapper;

    //[SetUp]
    public void Setup()
    {
        
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MapperForTransactionsCreater>()));
        _accountReader = new AccountReader();
        _transactionsToCsv = new TransactionsCsvHelper();
        _calculationServices = new CalculationServices();
    }

    //[Test]
    public async Task CreateFakeTransactionsForDbToCsv()
    {

        var resultTransactions = new List<TransactionDtoToCsv>();
        
        var accountsDictionary = _accountReader.GetDictionaryOut(@"filePath");
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
        _transactionsToCsv.ConvertToCsv(resultTransactions.OrderBy(r => r.Date).ToList(), @"filePath");
    }

    //[Test]
    public static void BulkInsertTransactions()
    {

        var transactionsHelper = new TransactionsCsvHelper();
        DataTable tbl = new DataTable();
        tbl.Columns.Add(new DataColumn("AccountId", typeof(int)));
        tbl.Columns.Add(new DataColumn("Date", typeof(DateTime)));
        tbl.Columns.Add(new DataColumn("TransactionType", typeof(Enum)));
        tbl.Columns.Add(new DataColumn("Amount", typeof(Decimal)));
        tbl.Columns.Add(new DataColumn("Currency", typeof(Enum)));

        var transactionsModel = transactionsHelper.ConvertToModel(@"filePath"); //50
        for (int i = 0; i < transactionsModel.Count; i++)
        {
            DataRow dr = tbl.NewRow();
            dr["AccountId"] = transactionsModel[i].AccountId;
            dr["Date"] = transactionsModel[i].Date;
            dr["TransactionType"] = transactionsModel[i].TransactionType;
            dr["Amount"] = transactionsModel[i].Amount;
            dr["Currency"] = transactionsModel[i].Currency;
            tbl.Rows.Add(dr);
        }

        string connection = @"??";
        SqlConnection con = new SqlConnection(connection);
        SqlBulkCopy objbulk = new SqlBulkCopy(con);

        objbulk.DestinationTableName = "[TransactionStore.DB].[dbo].[Transaction]";
        objbulk.ColumnMappings.Add("AccountId", "AccountId");
        objbulk.ColumnMappings.Add("Date", "Date");
        objbulk.ColumnMappings.Add("TransactionType", "TransactionType");
        objbulk.ColumnMappings.Add("Amount", "Amount");
        objbulk.ColumnMappings.Add("Currency", "Currency");

        con.Open();
        objbulk.WriteToServer(tbl);
        con.Close();
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
}