using System.Net;
using System.Text.Json;
using T_Strore.Business.Exceptions;
using NLog;

namespace T_Store.Middleware;

public class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Logger _logger;

    public CustomExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
        _logger = LogManager.GetCurrentClassLogger();
    }
       
    

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (EntityNotFoundException exception)
        {
            _logger.Error ($"TESt TEST TEST TEST Stopped program because of {exception}");
            await HandleExceptionAsync(context, HttpStatusCode.NotFound, exception.Message);
        }
        catch (BadRequestException exception)
        {
            _logger.Error(exception, $"Stopped program because of {exception}");
            await HandleExceptionAsync(context, HttpStatusCode.BadRequest, exception.Message);
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
