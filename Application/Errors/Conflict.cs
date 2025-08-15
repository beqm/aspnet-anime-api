using FluentResults;

namespace Application.Errors;

public class Conflict : Error
{
    public Conflict(string message) : base(message)
    {
        WithMetadata("StatusCode", 409);
    }
}