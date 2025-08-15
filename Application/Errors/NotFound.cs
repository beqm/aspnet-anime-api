using FluentResults;

namespace Application.Errors;

public class NotFound : Error
{
    public NotFound(string message) : base(message)
    {
        WithMetadata("StatusCode", 404);
    }
}