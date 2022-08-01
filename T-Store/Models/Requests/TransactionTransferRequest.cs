using System.ComponentModel.DataAnnotations;
using T_Store.CustomAttributesValidations;
using T_Store.Infrastructure;
using T_Strore.Data;

namespace T_Store.Models.Requests;

public class TransactionTransferRequest
{
    [CheckerNumberMoreZero]
    public int AccountIdSender { get; set; }

    [CheckerNumberMoreZero]
    public int Amount { get; set; }

    [CheckerNumberMoreZero]
    public int AccountIdRecipient { get; set; }

    [Range(1, 7, ErrorMessage = ApiErrorMessage.CurrencyRangeError)]
    public Currency CurrencyRecipient { get; set; }
}
