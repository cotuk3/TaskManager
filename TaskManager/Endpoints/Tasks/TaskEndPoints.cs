using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.Api.Endpoints.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Application.Tasks;

namespace TaskManager.Api.EndPoints.Tasks;

public static class TaskEndPoints
{
    public static void AddTaskEndPoints(this WebApplication app)
    {
        app.MapGet("tasks", GetMyTasks)
            .WithTags("Tasks")
            .RequireAuthorization();

        app.MapGet("tasks/{id}", GetTaskById)
            .WithTags("Tasks")
            .RequireAuthorization();

        app.MapPost("tasks", CreateTask)
            .WithTags("Tasks")
            .RequireAuthorization();

        app.MapPut("tasks/{id}/complete", CompleteTask)
            .WithTags("Tasks")
            .RequireAuthorization();

        app.MapDelete("tasks/{id}", DeleteTask)
            .WithTags("Tasks")
            .RequireAuthorization();
    }

    public static async Task<IResult> GetMyTasks([FromServices] GetTasksByUserService service, ClaimsPrincipal user,
        CancellationToken cancellationToken)
    {
        string userId = user.FindFirstValue(ClaimTypes.NameIdentifier)
             ?? throw new UnauthorizedAccessException();

        var tasks = await service.ExecuteAsync(userId, cancellationToken);

        return Results.Ok(tasks);
    }

    public static async Task<IResult> GetTaskById([FromServices] GetTaskByIdService service, Guid id, CancellationToken cancellationToken)
    {
        var tasks = await service.ExecuteAsync(id, cancellationToken);

        return Results.Ok(tasks);
    }

    public static async Task<IResult> CreateTask([FromServices] CreateTaskService service, CreateTaskRequest request, ClaimsPrincipal user, CancellationToken cancellationToken)
    {
        string userId = user.FindFirstValue(ClaimTypes.NameIdentifier)
             ?? throw new UnauthorizedAccessException();

        CreateTaskDto createTaskDto = new()
        {
            UserId = userId,
            Description = request.Description,
            DueDate = request.DueDate,
            Labels = request.Labels
        };

        await service.ExecuteAsync(createTaskDto, cancellationToken);

        return Results.Created();
    }

    public static async Task<IResult> CompleteTask([FromServices] CompleteTaskService service, Guid id, CancellationToken cancellationToken)
    {
        await service.ExecuteAsync(id, cancellationToken);

        return Results.Ok();
    }

    public static async Task<IResult> DeleteTask([FromServices] DeleteTaskService service, Guid id, CancellationToken cancellationToken)
    {
        await service.ExecuteAsync(id, cancellationToken);

        return Results.NoContent();
    }
}
