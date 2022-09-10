namespace T_Strore.Business.Exceptions;

public class EntityNotFoundException : Exception 
{
    public EntityNotFoundException(string message) : base(message) { }
}
