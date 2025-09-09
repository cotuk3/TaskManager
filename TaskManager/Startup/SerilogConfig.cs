using Serilog;
using Serilog.Formatting.Json;

namespace TaskManager.Api.Startup;

public static class SerilogConfig
{
    public static void AddSerilogConfig(this IHostBuilder host)
    {
        host.UseSerilog((ctx, lc) => lc
            .ReadFrom.Configuration(ctx.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "TaskManager")
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"
            )
            .WriteTo.File(
                path: "Logs/log-.json",
                rollingInterval: RollingInterval.Day,
                formatter: new JsonFormatter(renderMessage: true)
            )
        );
    }
}
