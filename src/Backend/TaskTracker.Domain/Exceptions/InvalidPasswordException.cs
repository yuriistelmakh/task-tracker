namespace TaskTracker.Domain.Exceptions;

public class InvalidPasswordException : DomainException
{
    public InvalidPasswordException(string message) : base(message)
    {
    }
}
