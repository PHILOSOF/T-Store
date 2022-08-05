using T_Strore.Data;

namespace T_Store.Models.Responses
{
    public class TransferResponse: TransactionResponse
    {
        public int IdRecipient { get; set; }
        public int AccountIdRecipient { get; set; }
        public decimal AmountRecipient { get; set; }
        public Currency CurrencyRecipient { get; set; }
    }
}
