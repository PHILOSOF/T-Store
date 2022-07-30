using T_Strore.Data;

namespace T_Store.Models.Requests;

public class TransactionRequest
{
    public int AccountId { get; set; }
    public decimal Amount { get; set; }
    
}
