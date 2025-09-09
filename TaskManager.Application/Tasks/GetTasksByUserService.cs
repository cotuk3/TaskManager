using Microsoft.Extensions.Logging;
using TaskManager.Core.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions.DomainExceptions;
using TaskManager.Domain.Exceptions.InfrastractureExceptions;

namespace TaskManager.Application.Tasks;
public class GetTasksByUserService(IRepository<TaskItem> repository, ILogger<GetTasksByUserService> logger)
{
    private readonly IRepository<TaskItem> _taskRepo = repository;
    private readonly ILogger<GetTasksByUserService> _logger = logger;
    public async Task<IEnumerable<TaskItem>> ExecuteAsync(string userId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching tasks for user: {UserId}", userId);

        if(string.IsNullOrWhiteSpace(userId))
            throw new UserNotFoundException("null");

        var tasks = await _taskRepo.GetAllAsync(cancellationToken);

        if(tasks is null)
            throw new RepositoryException("Task repository returned null.");

        var userTasks = tasks.Where(t => t.UserId == userId).ToList();

        if(!userTasks.Any())
            throw new NoTasksFoundForUserException(userId);

        _logger.LogInformation("Found {Count} tasks for user {UserId}", userTasks.Count, userId);

        return userTasks;
    }

}
