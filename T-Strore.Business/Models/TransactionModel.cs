using IncredibleBackendContracts.Enums;

namespace T_Strore.Business.Models;

public class TransactionModel
{
    public long Id { get; set; }
    public long AccountId { get; set; }
    public DateTime Date { get; set; }
    public TransactionType TransactionType { get; set; }
    public decimal Amount { get; set; }
    public Currency Currency { get; set; }
}
