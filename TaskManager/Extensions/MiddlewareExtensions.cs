using TaskManager.Api.Middleware;

namespace TaskManager.Api.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestLoggingMiddleware>();

        return app;
    }

    public static IApplicationBuilder UseExceptionHandlerMidd(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        return app;
    }

}
