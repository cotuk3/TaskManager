using Microsoft.Extensions.Logging;
using TaskManager.Core.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Exceptions.DomainExceptions;

namespace TaskManager.Application.Tasks;
public class GetTaskByIdService(IRepository<TaskItem> repository, ILogger<GetTaskByIdService> logger)
{
    private readonly IRepository<TaskItem> _taskRepo = repository;
    private readonly ILogger<GetTaskByIdService> _logger = logger;

    public async Task<TaskItem?> ExecuteAsync(Guid taskId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Executing GetTaskByIdService for TaskId: {TaskId}", taskId);

        string id = taskId.ToString();

        if(string.IsNullOrWhiteSpace(id))
        {
            _logger.LogWarning("TaskId is null or empty");
            throw new TaskNotFoundException("null");
        }
        TaskItem? task = await _taskRepo.GetByIdAsync(id, cancellationToken);

        if(task is null)
        {
            _logger.LogWarning("Task not found: {TaskId}", taskId);
            throw new TaskNotFoundException(id);
        }

        _logger.LogInformation("Task found: {TaskId}", task.Id);

        return task;
    }
}
