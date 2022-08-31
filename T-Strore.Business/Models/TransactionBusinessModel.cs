using T_Strore.Data;

namespace T_Strore.Business.Models;

public class TransactionModel
{
    public long Id { get; set; }
    public long AccountId { get; set; }
    public DateTime Date { get; set; }
    public TransactionType TransactionType { get; set; }
    public decimal Amount { get; set; }
    public Currency Currency { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is TransactionModel model &&
               Id == model.Id &&
               AccountId == model.AccountId &&
               Date == model.Date &&
               Amount == model.Amount &&
               TransactionType == model.TransactionType &&
               Amount == model.Amount &&
               Currency == model.Currency;
    }
}
