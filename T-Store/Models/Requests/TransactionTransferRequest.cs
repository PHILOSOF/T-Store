using T_Store.CustomValidations;
using T_Strore.Data;

namespace T_Store.Models;

public class TransactionTransferRequest : TransactionRequest
{

    public int RecipientAccountId { get; set; }

    public Currency RecipientCurrency{ get; set; }
}
