namespace DeskFlow.API.Exceptions;

// Recurso não encontrado -> 404
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}
