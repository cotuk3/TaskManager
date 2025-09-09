namespace TaskManager.Tests.Common;
using System.Security.Claims;

public static class TestClaimsFactory
{
    public static ClaimsPrincipal Create(string userId) =>
        new ClaimsPrincipal(new ClaimsIdentity(
            new[] { new Claim(ClaimTypes.NameIdentifier, userId) }, "mock"));
}
