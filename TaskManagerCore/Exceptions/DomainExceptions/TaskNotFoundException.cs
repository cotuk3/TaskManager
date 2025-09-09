using Microsoft.AspNetCore.Http;

namespace TaskManager.Domain.Exceptions.DomainExceptions;
public class TaskNotFoundException : DomainException
{
    public TaskNotFoundException(string taskId)
        : base($"Task '{taskId}' not found.", "TASK_NOT_FOUND", StatusCodes.Status404NotFound) { }

}
