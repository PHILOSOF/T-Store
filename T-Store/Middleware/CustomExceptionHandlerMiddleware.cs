using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using T_Strore.Business.Exceptions;

namespace T_Store.Middleware;


public class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

    public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (EntityNotFoundException exception)
        {
            _logger.LogError($"Stopped program because of {exception}");
            await HandleExceptionAsync(context, HttpStatusCode.NotFound, exception.Message);
        }
        catch (BalanceExceedException exception)
        {
            _logger.LogError(exception, $"Stopped program because of {exception}");
            await HandleExceptionAsync(context, HttpStatusCode.BadRequest, exception.Message);
        }
        catch (ServiceUnavailableException exception)
        {
            _logger.LogError(exception, $"Stopped program because of {exception}");
            await HandleExceptionAsync(context, HttpStatusCode.ServiceUnavailable, exception.Message);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var result = JsonSerializer.Serialize(new { error = message });

        return context.Response.WriteAsync(result);
    }
}
