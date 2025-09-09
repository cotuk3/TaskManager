using Scalar.AspNetCore;
using TaskManager.Api.Extensions;

namespace TaskManager.Startup;

public static class OpenApiConfig
{
    public static void AddOpenApiServices(this IServiceCollection services)
    {
        services.AddOpenApi();
    }

    public static void UseOpenApi(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options.Title = "Task Manager API";
                options.HideClientButton = true;
                options.Theme = ScalarTheme.Purple;
            });

            app.ApplyMigrations();

            app.UseRequestLogging();
            app.UseExceptionHandlerMidd();
        }
    }
}
