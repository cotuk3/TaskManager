using Serilog.Context;
using System.Diagnostics;
using System.Security.Claims;

namespace TaskManager.Api.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var requestId = Guid.NewGuid().ToString();
        var userId = context.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Anonymous";

        var sw = Stopwatch.StartNew();

        using(LogContext.PushProperty("RequestId", requestId))
        using(LogContext.PushProperty("UserId", userId))
        {
            _logger.LogInformation("Incoming {Method} {Path}", context.Request.Method, context.Request.Path);

            await _next(context);

            sw.Stop();

            _logger.LogInformation(
                "{Method} {Path} responded {StatusCode} in {Elapsed} ms (User: {UserId})",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                sw.ElapsedMilliseconds,
                userId
            );
        }
    }
}
