
namespace T_Strore.Data;

public class TransactionDto
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public DateTime Date { get; set; }
    public TransactionType TransactionType { get; set; }
    public decimal Amount { get; set; }
    public Currency Currency { get; set; }

    //public override bool Equals(object? obj)
    //{

    //    return obj is TransactionDto dto &&
    //        Id == dto.Id &&
    //        AccountId == dto.AccountId &&
    //        Date == dto.Date &&
    //        TransactionType == dto.TransactionType &&
    //        Amount == dto.Amount &&
    //        Currency == dto.Currency;
    //}
}


