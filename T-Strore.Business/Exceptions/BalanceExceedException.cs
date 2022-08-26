namespace T_Strore.Business.Exceptions;

public class BalanceExceedException : Exception
{
    public BalanceExceedException(string message) : base(message) { }
}