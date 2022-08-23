using T_Store.Middleware;


namespace T_Store.Extensions
{
    public static class AdminSafeListMiddlewareExtensions
    {
        public static IApplicationBuilder UseAdminSafeList(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AdminSafeListMiddleware>(SafeIps.Local);
        }
    }
}
