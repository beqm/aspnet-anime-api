using FluentResults;

namespace Application.Errors;

public class BadRequest : Error
{
    public BadRequest(string message) : base(message)
    {
        WithMetadata("StatusCode", 400);
    }
}