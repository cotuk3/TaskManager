using TaskManager.Domain.Exceptions.DomainExceptions;
using TaskManager.Domain.Exceptions.InfrastractureExceptions;

namespace TaskManager.Api.Middleware;

/*public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext ctx)
    {
        try
        {
            await _next(ctx);
        }
*//*        catch(DomainException ex)
        {
            _logger.LogWarning(ex, "Domain error");
            ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
            await ctx.Response.WriteAsJsonAsync(new { error = ex.Message });
        }*//*
        catch(Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await ctx.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred." });
        }
    }
}

*/
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch(InfrastructureException ex)
        {
            _logger.LogError(ex, "Infrastructure error: {ErrorCode}", ex.ErrorCode);

            context.Response.StatusCode = ex.StatusCode;
            await context.Response.WriteAsJsonAsync(new
            {
                error = "A technical error occurred. Please try again later.",
                code = ex.ErrorCode,
                status = ex.StatusCode
            });
        }
        catch(DomainException ex)
        {
            _logger.LogWarning(ex, "Business error: {ErrorCode}", ex.ErrorCode);

            context.Response.StatusCode = ex.StatusCode;
            await context.Response.WriteAsJsonAsync(new
            {
                error = ex.Message,
                code = ex.ErrorCode,
                status = ex.StatusCode
            });
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Unexpected error occurred.",
                code = "UNEXPECTED_ERROR",
                status = StatusCodes.Status500InternalServerError
            });
        }
    }
}