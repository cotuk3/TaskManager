using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using TaskManager.Api.Startup;
using TaskManager.Application.Tasks;
using TaskManager.Application.Users;
using TaskManager.Core.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Infrastracture.Repositories;

namespace TaskManager.Startup;

public static class DependenciesConfig
{
    public static void AddDependencies(this WebApplicationBuilder builder)
    {
        builder.Host.AddSerilogConfig();

        builder.Services.AddAuthorizationConfig();

        builder.Services.AddAuthenticationConfig();

        builder.Services.AddOpenApiServices();

        builder.Services.AddDatabaseConfig(builder.Configuration);
        builder.Services.AddIdentityConfig();

        builder.Services.AddScoped<IRepository<User>, UserRepository>();
        builder.Services.AddScoped<IRepository<TaskItem>, TaskRepository>();


        builder.Services.AddScoped<GetTasksByUserService>();
        builder.Services.AddScoped<GetTaskByIdService>();
        builder.Services.AddScoped<CompleteTaskService>();
        builder.Services.AddScoped<CreateTaskService>();
        builder.Services.AddScoped<DeleteTaskService>();
        builder.Services.AddScoped<GetUserByIdService>();

    }
}
