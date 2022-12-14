using T_Store.Middleware;


namespace T_Store.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseAdminSafeList(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AdminSafeListMiddleware>();
    }

    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }
}
