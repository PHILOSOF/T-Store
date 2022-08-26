using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace T_Store.Middleware;

[ExcludeFromCodeCoverage]
public class AdminSafeListMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AdminSafeListMiddleware> _logger;
    private readonly string[] _safelist;

    public AdminSafeListMiddleware(RequestDelegate next, ILogger<AdminSafeListMiddleware> logger, IConfiguration configuration)
    {
        // call helper to get IpWhilelist from configuration

        _safelist = "whitelist".Split(';');

        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var remoteIp = context.Connection.RemoteIpAddress;
        // consider usage of "context.Request.Host"

        _logger.LogInformation($"Request from Remote IP address: {remoteIp}");

        if (!_safelist.Contains(remoteIp!.ToString()))
        {
            _logger.LogWarning($"Forbidden Request from Remote IP address: {remoteIp}");
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return;
        }
        await _next.Invoke(context);
    }
}
