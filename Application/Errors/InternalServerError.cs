using FluentResults;

namespace Application.Errors;

public class InternalServerError : Error
{
    public InternalServerError(string message) : base(message)
    {
        WithMetadata("StatusCode", 500);
    }
}