using Microsoft.Extensions.Logging;
using TaskManager.Core.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Tasks;
public class DeleteTaskService(IRepository<TaskItem> repository, ILogger<DeleteTaskService> logger)
{
    private readonly IRepository<TaskItem> _taskRepo = repository;
    private readonly ILogger<DeleteTaskService> _logger = logger;

    public async Task ExecuteAsync(Guid Id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting task with ID: {TaskId}", Id);

        await _taskRepo.Delete(Id.ToString(), cancellationToken);

        _logger.LogInformation("Task with ID: {TaskId} deleted successfully", Id);
    }
}
