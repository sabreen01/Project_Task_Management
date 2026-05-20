namespace Core.Domain.Exceptions;

public class DomainException : Exception
{
    public string ErrorCode { get; }

    public DomainException(string message) : base(message)
    {
        ErrorCode = "DomainError";
    }
    
    public DomainException(string errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}
