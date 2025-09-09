using Microsoft.Extensions.Logging;
using TaskManager.Core.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Tasks;
public class CreateTaskService(IRepository<TaskItem> repository, ILogger<CreateTaskService> logger)
{
    private readonly IRepository<TaskItem> _taskRepo = repository;
    private readonly ILogger<CreateTaskService> _logger = logger;

    public async Task ExecuteAsync(CreateTaskDto dto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating task for user: {UserId}", dto.UserId);

        var taskItem = new TaskItem
        {
            Id = Guid.NewGuid(),
            UserId = dto.UserId,
            Description = dto.Description,
            Priority = dto.Priority,
            DueDate = new DueDate(dto.DueDate),
            Labels = dto.Labels,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        _logger.LogInformation("Task created with ID: {TaskId}", taskItem.Id);

        await _taskRepo.AddAsync(taskItem, cancellationToken);
    }
}


