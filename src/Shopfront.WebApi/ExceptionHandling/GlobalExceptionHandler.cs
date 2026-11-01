using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Shopfront.Application.Exceptions;
using Shopfront.Domain.Exceptions;

namespace Shopfront.WebApi.ExceptionHandling;

public sealed class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private const string UnexpectedErrorDetail = "An unexpected error occurred while processing the request";

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var problem = exception switch
        {
            ValidationException validation => CreateValidationProblem(validation),
            NotFoundException => CreateProblem(StatusCodes.Status404NotFound, "Resource not found", exception.Message),
            DomainException => CreateProblem(StatusCodes.Status422UnprocessableEntity, "Business rule violated", exception.Message),
            _ => CreateProblem(StatusCodes.Status500InternalServerError, "Server error", UnexpectedErrorDetail)
        };

        if (problem.Status == StatusCodes.Status500InternalServerError)
        {
            logger.LogError(
                exception,
                "Unhandled exception during {RequestMethod} {RequestPath}",
                httpContext.Request.Method,
                httpContext.Request.Path);
        }

        httpContext.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problem,
            Exception = exception
        });
    }

    private static ProblemDetails CreateProblem(int status, string title, string detail) => new()
    {
        Status = status,
        Title = title,
        Detail = detail
    };

    private static ProblemDetails CreateValidationProblem(ValidationException exception)
    {
        var errors = exception.Errors
            .GroupBy(failure => failure.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(failure => failure.ErrorMessage).ToArray());

        return new HttpValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation failed"
        };
    }
}
