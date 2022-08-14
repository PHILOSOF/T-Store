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
        return null + ";"+
            AccountId + ";" + 
            Date.ToString() + ";" + 
            ((int)TransactionType).ToString() + ";" + 
            Amount.ToString() + ";" + 
            ((int)Currency).ToString();
    }
}
