using System.Globalization;
using T_Strore.Data;

namespace TransactionsCreater.Model;

public class TransactionDtoToCsv : TransactionDto
{
    public long LeadId { get; set; }


    public TransactionDtoToCsv()
    {

    }

    public TransactionDtoToCsv(TransactionDtoToCsv transactionDtoToCsv)
    {
        Id = transactionDtoToCsv.Id;
        AccountId = transactionDtoToCsv.AccountId;
        Date = transactionDtoToCsv.Date;
        TransactionType = transactionDtoToCsv.TransactionType;
        Amount = transactionDtoToCsv.Amount;
        Currency = transactionDtoToCsv.Currency;
        LeadId = transactionDtoToCsv.LeadId;

    }

    public String ToCsvRow()
    {
        var a = Date.ToLongDateString();
        return null + ";"+
            AccountId + ";" + 
            Date.ToString() +"." + Date.Millisecond.ToString()+ ";" + 
            ((int)TransactionType).ToString() + ";" +
            ((Amount.ToString()).Replace(",",".")) + ";" + 
            ((int)Currency).ToString();
    }
}
