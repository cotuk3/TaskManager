using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Moq;
using TaskManager.Api.Endpoints.Users;
using TaskManager.Application.Users;
using TaskManager.Core.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions.DomainExceptions;
using TaskManager.Tests.Common;

namespace TaskManager.Tests.EndPoints;
public class GetUserByIdEndpointTests
{
    [Fact]
    public async Task ShouldReturnOk_WhenUserExists()
    {
        var expectedUser = new User()
        {
            Id = "user-123",
            UserName = "testuser",
            Email = "my@gmail.com"
        };
        var repoMock = new Mock<IRepository<User>>();
        var loggerMock = new Mock<ILogger<GetUserByIdService>>();


        repoMock.Setup(r => r.GetByIdAsync(expectedUser.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync(expectedUser);

        var service = new GetUserByIdService(repoMock.Object, loggerMock.Object);

        var claims = TestClaimsFactory.Create("user-123");

        var result = await UserEndPoints.GetUserById(service, claims, CancellationToken.None);

        result.Should().BeOfType<Ok<string>>();
    }

    [Fact]
    public async Task ShouldThrow_UserNotFoundException_WhenUserIdIsEmpty()
    {
        var repoMock = new Mock<IRepository<User>>();
        var loggerMock = new Mock<ILogger<GetUserByIdService>>();


        repoMock.Setup(r => r.GetByIdAsync("", It.IsAny<CancellationToken>()))
        .ReturnsAsync(new User());

        var service = new GetUserByIdService(repoMock.Object, loggerMock.Object);

        var claims = TestClaimsFactory.Create("");

        Func<Task> act = async () => await UserEndPoints.GetUserById(service, claims, CancellationToken.None);

        await act.Should().ThrowAsync<UserNotFoundException>();
    }

    [Fact]
    public async Task ShouldThrow_UserNotFoundException_WhenUserIsNull()
    {
        var repoMock = new Mock<IRepository<User>>();
        var loggerMock = new Mock<ILogger<GetUserByIdService>>();


        repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
        .ReturnsAsync((IEnumerable<User>)null!);

        var service = new GetUserByIdService(repoMock.Object, loggerMock.Object);

        var claims = TestClaimsFactory.Create("user-123");

        Func<Task> act = async () => await UserEndPoints.GetUserById(service, claims, CancellationToken.None);

        await act.Should().ThrowAsync<UserNotFoundException>();
    }

}