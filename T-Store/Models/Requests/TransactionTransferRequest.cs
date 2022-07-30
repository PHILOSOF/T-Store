using T_Strore.Data;

namespace T_Store.Models.Requests;

public class TransactionTransferRequest : TransactionRequest
{
    public int AccountIdRecipient { get; set; }
    public Currency CurrencyRecipient { get; set; }
}
