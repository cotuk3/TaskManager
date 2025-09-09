using Microsoft.AspNetCore.Http;

namespace TaskManager.Domain.Exceptions.DomainExceptions;
public class NoTasksFoundForUserException : DomainException
{
    public NoTasksFoundForUserException(string userId)
        : base($"No tasks found for user '{userId}'.", "NO_TASKS_FOR_USER", StatusCodes.Status404NotFound) { }
}
