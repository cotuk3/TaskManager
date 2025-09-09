using Microsoft.EntityFrameworkCore;
using TaskManager.Infrastracture.Data;
using TaskManager.Infrastracture.Identity;

namespace TaskManager.Api.Extensions;

public static class MigrationExtensions
{

    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using TaskManagerDbContext dbContext =
            scope.ServiceProvider.GetRequiredService<TaskManagerDbContext>();

        dbContext.Database.Migrate();
    }
}
