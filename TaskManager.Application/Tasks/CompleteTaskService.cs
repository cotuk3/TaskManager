using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Exceptions.DomainExceptions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TaskManager.Application.Tasks;
public class CompleteTaskService(IRepository<TaskItem> repository, ILogger<CompleteTaskService> logger)
{
    private readonly IRepository<TaskItem> _taskRepo = repository;
    private readonly ILogger<CompleteTaskService> _logger = logger;
    public async Task ExecuteAsync(Guid taskId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Completing task: {TaskId}", taskId);

        var task = await _taskRepo.GetByIdAsync(taskId.ToString(), cancellationToken);

        if(task is null)
        {
            _logger.LogWarning("Task not found: {TaskId}", taskId);
            throw new TaskNotFoundException(taskId.ToString());
        }

        if(task.IsCompleted)
        {
            _logger.LogWarning("Task already completed: {TaskId}", taskId);
            throw new TaskAlreadyCompletedException(taskId);
        }

        task.IsCompleted = true;
        task.CompletedAt = DateTime.UtcNow;

        await _taskRepo.UpdateAsync(task, cancellationToken);

        _logger.LogInformation("Task marked as completed: {TaskId}", taskId);
    }

}
