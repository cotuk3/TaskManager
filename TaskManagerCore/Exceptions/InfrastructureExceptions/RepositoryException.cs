using Microsoft.AspNetCore.Http;

namespace TaskManager.Domain.Exceptions.InfrastractureExceptions;

public class RepositoryException : InfrastructureException
{
    public RepositoryException(string message)
        : base(message, "REPOSITORY_ERROR", StatusCodes.Status500InternalServerError) { }
}