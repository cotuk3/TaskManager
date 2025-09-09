using Microsoft.AspNetCore.Http;

namespace TaskManager.Domain.Exceptions.DomainExceptions;
public class UserNotFoundException : DomainException
{
    public UserNotFoundException(string userId)
        : base($"User with ID '{userId}' not found.", "USER_NOT_FOUND", StatusCodes.Status404NotFound) { }

}
