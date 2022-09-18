using System.Diagnostics.CodeAnalysis;
using System.Net;


namespace T_Store.Middleware;


public class AdminSafeListMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AdminSafeListMiddleware> _logger;
    private readonly string[] _safelist;

    public AdminSafeListMiddleware(RequestDelegate next, ILogger<AdminSafeListMiddleware> logger, IConfiguration configuration)
    {
        var whitList = configuration.GetSection("HostWhiteList").Value.ToString();
        _safelist = whitList.Split(';'); 
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var remote = context.Request.Host;
        _logger.LogInformation($"Request from Remote host address: {remote.Host}");
    
        if(!_safelist.Contains(remote.Host))
        {
            _logger.LogError($"Forbidden Request from Remote IP address: {remote.Host}");
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return;
        }
        await _next.Invoke(context);
    }
}
