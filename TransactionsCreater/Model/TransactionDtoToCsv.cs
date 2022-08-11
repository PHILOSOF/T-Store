using T_Strore.Data;

namespace TransactionsCreater.Model;

public class TransactionDtoToCsv : TransactionDto
{
    public int LeadId { get; set; }

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
