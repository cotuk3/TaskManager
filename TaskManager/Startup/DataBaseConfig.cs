using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Infrastracture.Data;

namespace TaskManager.Api.Startup;

public static class DataBaseConfig
{
    public static void AddDatabaseConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<TaskManagerDbContext>(options => options.UseSqlServer(connectionString));
    }

    public static void AddIdentityConfig(this IServiceCollection services)
    {
        services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<TaskManagerDbContext>()
            .AddApiEndpoints();
    }
}
