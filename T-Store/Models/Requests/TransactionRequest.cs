using T_Store.CustomValidations;
using T_Strore.Data;

namespace T_Store.Models;


public class TransactionRequest
{
    [CheckerNumberMoreZero]
    public int AccountId { get; set; }
    public Currency Currency { get; set; }

    [CheckerNumberMoreZero]
    public decimal Amount { get; set; }
}
