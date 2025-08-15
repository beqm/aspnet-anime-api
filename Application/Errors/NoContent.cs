using FluentResults;

namespace Application.Errors;

public class NoContent : Error
{
    public NoContent(string message) : base(message)
    {
        WithMetadata("StatusCode", 204);
    }
}