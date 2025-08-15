using FluentResults;

public static class ResultExtensions
{
    public static IResult ToIResult(this Result result)
    {
        if (result.IsSuccess)
        {
            return Results.NoContent();
        }

        return BuildProblemResult(result.Errors);
    }

    public static IResult ToIResult<T>(this Result<T> result, int? statusCode = null)
    {
        if (result.IsSuccess)
        {
            statusCode = statusCode ?? StatusCodes.Status200OK;
            return Results.Json(result.Value, statusCode: statusCode);
        }

        return BuildProblemResult(result.Errors);
    }

    private static IResult BuildProblemResult(IReadOnlyList<IError> errors)
    {
        var codeFromMetadata = errors
            .SelectMany(e => e.Metadata)
            .Where(m => m.Key == "StatusCode" && m.Value is int)
            .Select(m => (int)m.Value)
            .FirstOrDefault();

        var statusCode = codeFromMetadata != 0 ? codeFromMetadata : StatusCodes.Status400BadRequest;

        var errorList = errors.Select(e => new Dictionary<string, object?>
        {
            ["Message"] = e.Message,
        }).ToList();

        var extensions = new Dictionary<string, object?>
        {
            ["Errors"] = errorList
        };

        return Results.Problem(
            detail: string.Join("; ", errors.Select(e => e.Message)),
            statusCode: statusCode,
            title: "Operation Failed",
            extensions: extensions
        );
    }
}
