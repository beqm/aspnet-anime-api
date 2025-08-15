using Serilog;
using System.Net;
using System.Text.Json;
using FluentResults;

namespace Api.Middleware;

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<object> Errors { get; set; } = new();
    public string TraceId { get; set; } = string.Empty;
}

public class ErrorMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);

            if (context.Items.TryGetValue("FluentResult", out var resultObj) && resultObj is Result result)
            {
                await WriteFluentResultAsync(context, result);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unhandled exception");
            await HandleGenericExceptionAsync(context, ex);
        }
    }

    private static async Task WriteFluentResultAsync(HttpContext context, Result result)
    {
        if (result.IsSuccess)
        {
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NoContent;
            }
            return;
        }

        var firstStatus = result.Errors
            .SelectMany(e => e.Metadata)
            .Where(m => m.Key == "StatusCode" && m.Value is int)
            .Select(m => (int)m.Value)
            .FirstOrDefault();

        var statusCode = firstStatus != 0 ? firstStatus : (int)HttpStatusCode.BadRequest;

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var errorList = result.Errors.Select(e => new
        {
            e.Message,
            e.Metadata
        }).ToList();

        var response = new ErrorResponse
        {
            StatusCode = statusCode,
            Message = "Operation failed",
            Errors = errorList.Cast<object>().ToList()
        };

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }

    private static async Task HandleGenericExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new ErrorResponse
        {
            StatusCode = context.Response.StatusCode,
            Message = "An unexpected error occurred.",
            Errors = new List<object>
            {
                new { ex.Message }
            },
            TraceId = context.TraceIdentifier
        };

        Log.Error(ex, "Unhandled exception");

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}
