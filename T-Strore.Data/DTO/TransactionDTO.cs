
namespace T_Strore.Data
{
    public class TransactionDTO
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public DateTime Date { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }

        public override bool Equals(object? obj)
        {

            return obj is TransactionDTO dto &&
                Id == dto.Id &&
                AccountId == dto.AccountId &&
                Date == dto.Date &&
                Type == dto.Type &&
                Amount == dto.Amount &&
                Currency == dto.Currency;
        }
    }

   
}
