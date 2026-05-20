namespace Core.Application.Helper.Models;

public record RequestResult<T>(
    T? Data,
    bool IsSuccess,
    string Message
)
{
    public static RequestResult<T> Success(T data, string message = "Success") 
        => new(data, true, message);

    public static RequestResult<T> Failure(string message) 
        => new(default, false, message);
}
