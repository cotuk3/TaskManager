using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Api.EndPoints.Tasks;
using TaskManager.Application.Tasks;
using TaskManager.Core.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions.DomainExceptions;
using TaskManager.Domain.Exceptions.InfrastractureExceptions;
using TaskManager.Tests.Common;

namespace TaskManager.Tests.EndPoints;
public class GetTasksByUserEndpointTests
{
    [Fact]
    public async Task ShouldReturnOk_WhenTasksExist()
    {
        var expectedTasks = new List<TaskItem>
        {
            new TaskItem { Id = Guid.NewGuid(), Description = "Test", UserId = "user-123" }
        };

        var repoMock = new Mock<IRepository<TaskItem>>();
        var loggerMock = new Mock<ILogger<GetTasksByUserService>>();

        repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTasks);

        var service = new GetTasksByUserService(repoMock.Object, loggerMock.Object);

        var claims = TestClaimsFactory.Create("user-123");

        var result = await TaskEndPoints.GetMyTasks(service, claims, CancellationToken.None);

        result.Should().BeOfType<Ok<IEnumerable<TaskItem>>>();
    }

    [Fact]
    public async Task ShouldThrow_UserNotFoundException_WhenUserIdIsEmpty()
    {
        var repoMock = new Mock<IRepository<TaskItem>>();
        var loggerMock = new Mock<ILogger<GetTasksByUserService>>();

        var service = new GetTasksByUserService(repoMock.Object, loggerMock.Object);
        var claims = TestClaimsFactory.Create(""); // порожній userId

        Func<Task> act = async () => await TaskEndPoints.GetMyTasks(service, claims, CancellationToken.None);

        await act.Should().ThrowAsync<UserNotFoundException>();
    }

    [Fact]
    public async Task ShouldThrow_RepositoryException_WhenRepoReturnsNull()
    {
        var repoMock = new Mock<IRepository<TaskItem>>();
        repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<TaskItem>?)null);

        var loggerMock = new Mock<ILogger<GetTasksByUserService>>();
        var service = new GetTasksByUserService(repoMock.Object, loggerMock.Object);
        var claims = TestClaimsFactory.Create("user-123");

        Func<Task> act = async () => await TaskEndPoints.GetMyTasks(service, claims, CancellationToken.None);

        await act.Should().ThrowAsync<RepositoryException>();
    }

    [Fact]
    public async Task ShouldThrow_NoTasksFoundForUserException_WhenNoTasksExist()
    {
        var repoMock = new Mock<IRepository<TaskItem>>();
        repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TaskItem>()); // порожній список

        var loggerMock = new Mock<ILogger<GetTasksByUserService>>();
        var service = new GetTasksByUserService(repoMock.Object, loggerMock.Object);
        var claims = TestClaimsFactory.Create("user-123");

        Func<Task> act = async () => await TaskEndPoints.GetMyTasks(service, claims, CancellationToken.None);

        await act.Should().ThrowAsync<NoTasksFoundForUserException>();
    }
}
