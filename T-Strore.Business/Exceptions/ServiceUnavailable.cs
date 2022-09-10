namespace T_Strore.Business.Exceptions;

public class ServiceUnavailable : Exception
{
    public ServiceUnavailable(string message) : base(message) { }
}

