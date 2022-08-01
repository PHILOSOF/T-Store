using T_Strore.Data;

namespace T_Store.Models.Responses;

public class TransactionResponse
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public DateTime Date { get; set; }
    public TransactionType TransactionType { get; set; }
    public decimal Amount { get; set; }
    public Currency Currency { get; set; }
}
