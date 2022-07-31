using T_Strore.Data;

namespace T_Store.Models.Requests;

public class TransactionTransferRequest
{
    public int AccountIdSender { get; set; }
    public int Amount { get; set; }
    public int AccountIdRecipient { get; set; }
    public Currency CurrencyRecipient { get; set; }
}
