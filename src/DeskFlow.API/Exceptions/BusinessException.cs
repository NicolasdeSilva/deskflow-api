namespace DeskFlow.API.Exceptions;

// Regra de negócio violada -> 400
public class BusinessException : Exception
{
    public BusinessException(string message) : base(message) { }
}
