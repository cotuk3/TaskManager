using Microsoft.Extensions.Logging;
using TaskManager.Core.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions.DomainExceptions;

namespace TaskManager.Application.Users;
public class GetUserByIdService(IRepository<User> repository, ILogger<GetUserByIdService> logger)
{

    private readonly IRepository<User> _userRepo = repository;
    private readonly ILogger<GetUserByIdService> _logger = logger;

    public async Task<User> ExecuteAsync(string? userId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Executing GetUserByIdService for UserId: {UserId}", userId);

        if(string.IsNullOrWhiteSpace(userId))
        {
            _logger.LogWarning("UserId is null or empty");
            throw new UserNotFoundException("null");
        }

        var user = await _userRepo.GetByIdAsync(userId, cancellationToken);

        if(user is null)
        {
            _logger.LogWarning("User not found: {UserId}", userId);
            throw new UserNotFoundException(userId);
        }

        _logger.LogInformation("User found: {UserId}", user.Id);

        return user;
    }

}

