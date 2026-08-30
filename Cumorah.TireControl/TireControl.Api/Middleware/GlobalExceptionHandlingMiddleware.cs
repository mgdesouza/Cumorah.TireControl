using Microsoft.AspNetCore.Mvc;

namespace TireControl.Api.Middleware;

/// <summary>
/// Converts unhandled exceptions into RFC 7807 problem-details responses.
/// </summary>
public sealed class GlobalExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionHandlingMiddleware> logger,
    IHostEnvironment environment)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception) when (!context.Response.HasStarted)
        {
            var (statusCode, title) = GetResponseDetails(exception);
            var traceId = context.TraceIdentifier;

            logger.LogError(
                exception,
                "Unhandled exception while processing {Method} {Path}. TraceId: {TraceId}",
                context.Request.Method,
                context.Request.Path,
                traceId);

            context.Response.Clear();
            context.Response.StatusCode = statusCode;

            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = environment.IsDevelopment() ? exception.Message : null,
                Instance = context.Request.Path
            };
            problem.Extensions["traceId"] = traceId;

            await context.Response.WriteAsJsonAsync(problem, contentType: "application/problem+json");
        }
    }

    private static (int StatusCode, string Title) GetResponseDetails(Exception exception) => exception switch
    {
        BadHttpRequestException => (StatusCodes.Status400BadRequest, "Invalid request."),
        KeyNotFoundException => (StatusCodes.Status404NotFound, "Resource not found."),
        _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
    };
}
