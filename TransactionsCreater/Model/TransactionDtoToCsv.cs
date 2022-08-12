using T_Strore.Data;

namespace TransactionsCreater.Model;

internal class TransactionDtoToCsv : TransactionDto
{
    internal int LeadId { get; set; }


    internal TransactionDtoToCsv()
    {

    }

    internal TransactionDtoToCsv(TransactionDtoToCsv transactionDtoToCsv)
    {
        Id = transactionDtoToCsv.Id;
        AccountId = transactionDtoToCsv.AccountId;
        Date = transactionDtoToCsv.Date;
        TransactionType = transactionDtoToCsv.TransactionType;
        Amount = transactionDtoToCsv.Amount;
        Currency = transactionDtoToCsv.Currency;
        LeadId = transactionDtoToCsv.LeadId;
    }


    internal String ToCsvRow()
    {
        return null + ";"+
            AccountId + ";" + 
            Date.ToString() + ";" + 
            ((int)TransactionType).ToString() + ";" + 
            Amount.ToString() + ";" + 
            ((int)Currency).ToString();
    }
}
