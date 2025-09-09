using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace TaskManager.Api.Startup;

public static class AuthorizationConfig
{
    public static IServiceCollection AddAuthorizationConfig(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder(
                IdentityConstants.ApplicationScheme,
                IdentityConstants.BearerScheme)
                .RequireAuthenticatedUser()
                .Build();
        });
        return services;
    }
}
