using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Blog.Domain.DTOs;
using Blog.Domain.Exceptions;
using ValidationException = Blog.Domain.Exceptions.ValidationException;

namespace Blog.Api.Middleware;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next.Invoke(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    public Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = new Response()
        {
            Error = new ErrorResponse()
        };
        switch (exception)
        {
            case DomainException domainException:
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 400;
                response.Error.Errors.Add(new ErrorElement {Code = "DomainExceptionKey",Message = domainException.DomainError.ErrorMessage()});
                _logger.LogError(domainException, $"Error from {GetType().Namespace}. Status code: { context.Response.StatusCode}",context.Response.StatusCode,context.Request?.Headers["X-Correlation-ID"]);
                return context.Response.WriteAsync(JsonSerializer.Serialize(response.Error));
            case ArgumentException argumentException:
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 400;
                response.Error.Errors.Add(new ErrorElement { Code = "ArgumentExceptionKey", Message = argumentException.Message });
                return context.Response.WriteAsync(JsonSerializer.Serialize(response.Error));
            case ValidationException validationException:
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 400;
                return context.Response.WriteAsync(JsonSerializer.Serialize(validationException.Response.Error));
        }
        response.Error.Errors.Add(new ErrorElement(){Code = "InternalServerErrorKey",Message = "Internal server error"});
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 500;
        _logger.LogError(exception, $"Error from {GetType().Namespace}. Status code: { context.Response.StatusCode}",context.Response.StatusCode,context.Request?.Headers["X-Correlation-ID"]);
        return context.Response.WriteAsync(JsonSerializer.Serialize(response.Error));
    }
}