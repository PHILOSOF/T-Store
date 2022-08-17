using T_Strore.Data;

namespace T_Store.Models;

public class TransactionTransferRequest : TransactionRequest
{
    public long RecipientAccountId { get; set; }
    public Currency RecipientCurrency{ get; set; }
}
