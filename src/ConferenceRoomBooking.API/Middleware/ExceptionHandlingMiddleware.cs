using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

public sealed class ExceptionHandlingMiddleware(
    ILogger<ExceptionHandlingMiddleware> logger,
    IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title, detail, extensions) = exception switch
        {
            ValidationException valEx => (
                StatusCodes.Status400BadRequest,
                "Validation Error",
                "One or more validation errors occurred.",
                GetValidationErrors(valEx)
            ),
            DomainException domainEx => (
                StatusCodes.Status400BadRequest,
                "Domain Rule Violation",
                domainEx.Message,
                null
            ),
            NotFoundException notFoundEx => (
                StatusCodes.Status404NotFound,
                "Resource Not Found",
                notFoundEx.Message,
                null
            ),
            _ => (
                StatusCodes.Status500InternalServerError,
                "Server Error",
                "An unexpected error occurred on the server.",
                null
            )
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);
        }
        else
        {
            logger.LogWarning("Handled error [{Title}]: {Detail}", title, detail);
        }

        httpContext.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        if (extensions != null)
        {
            problemDetails.Extensions["errors"] = extensions;
        }

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails,
            Exception = exception
        });
    }

    private static Dictionary<string, string[]> GetValidationErrors(ValidationException exception)
    {
        return exception.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );
    }
}