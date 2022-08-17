using T_Strore.Data;

namespace T_Store.Models;

public class TransactionRequest
{
    public long AccountId { get; set; }
    public Currency Currency { get; set; }
    public decimal Amount { get; set; }
}
