using Serilog;
using System.Net;
using System.Text.Json;
using FluentValidation;
using Application.Common.Exceptions;

namespace Api.Middleware;

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();
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
        }
        catch (ValidationException ex)
        {
            Log.Error(ex, "Validation error");
            await HandleValidationExceptionAsync(context, ex);
        }
        catch (KeyNotFoundException ex)
        {
            Log.Error(ex, "Resource not found");
            await HandleNotFoundExceptionAsync(context, ex);
        }
        catch (ConflictException ex)
        {
            Log.Error(ex, "Conflict error");
            await HandleConflictExceptionAsync(context, ex);
        }
        catch (ApplicationException ex)
        {
            Log.Error(ex, "Application exception");
            await HandleApplicationExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unhandled exception");
            await HandleGenericExceptionAsync(context, ex);
        }
    }

    private static async Task HandleValidationExceptionAsync(HttpContext context, ValidationException ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var response = new ErrorResponse
        {
            StatusCode = context.Response.StatusCode,
            Message = "Validation failed.",
            Errors = ex.Errors.Select(e => e.ErrorMessage).ToList()
        };

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }

    private static async Task HandleNotFoundExceptionAsync(HttpContext context, KeyNotFoundException ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;

        var response = new ErrorResponse
        {
            StatusCode = context.Response.StatusCode,
            Message = ex.Message
        };

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }

    private static async Task HandleConflictExceptionAsync(HttpContext context, ConflictException ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.Conflict;

        var response = new ErrorResponse
        {
            StatusCode = context.Response.StatusCode,
            Message = ex.Message
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }


    private static async Task HandleApplicationExceptionAsync(HttpContext context, ApplicationException ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new ErrorResponse
        {
            StatusCode = context.Response.StatusCode,
            Message = ex.Message
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
            Message = "An unexpected error occurred."
        };

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}