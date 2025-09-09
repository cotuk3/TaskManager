using System.Security.Claims;
using TaskManager.Application.Users;
using TaskManager.Core.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Api.Endpoints.Users;

public static class UserEndPoints
{
    public static void AddUserEndPoints(this WebApplication app)
    {
        app.MapGet("users/me", GetUserById)
        .WithTags(Tags.Users).RequireAuthorization();
    }

    public static async Task<IResult> GetUserById(GetUserByIdService service, ClaimsPrincipal user, 
        CancellationToken cancellationToken)
    {
        string? userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var User = await service.ExecuteAsync(userId, cancellationToken);

        return Results.Ok(User.Id);
    }
}