using T_Strore.Data;

namespace T_Store.Models.Responses
{
    public class TransferResponse: TransactionResponse
    {
        public int RecipientId { get; set; }
        public int RecipientAccountId { get; set; }
        public decimal RecipientAmount { get; set; }
        public Currency RecipientCurrency { get; set; }
    }
}
