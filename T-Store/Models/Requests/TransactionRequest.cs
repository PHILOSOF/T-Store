using System.ComponentModel.DataAnnotations;
using T_Store.CustomAttributesValidations;
using T_Store.Infrastructure;
using T_Strore.Data;

namespace T_Store.Models;

public class TransactionRequest
{
    [CheckerNumberMoreZero]
    public int AccountId { get; set; }

    [Range(1, 7, ErrorMessage = ApiErrorMessage.CurrencyRangeError)]
    public Currency Currency { get; set; }

    [CheckerNumberMoreZero]
    public decimal Amount { get; set; }
    
}
