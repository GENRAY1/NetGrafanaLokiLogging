using FluentResults;

namespace Client.Errors;

public class ApiError : Error
{
    public ApiError(
        string message,
        string code) : base(message)
    {
        Metadata.Add("code", code);
    }
}