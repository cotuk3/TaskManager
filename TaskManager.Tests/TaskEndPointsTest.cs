namespace TaskManager.Tests;

using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Endpoints.Tasks;
using TaskManager.Api.EndPoints.Tasks;
using TaskManager.Application.Tasks;
using TaskManager.Core.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions.DomainExceptions;
using Xunit;

public class TaskEndPointsTests
{
    [Fact]
    public async Task GetTasksByUser_ShouldReturnOk_WithUserTasks()
    {
        // Arrange
        var userId = "user-123";
        var allTasks = new List<TaskItem>
        {
            new TaskItem { Id = Guid.NewGuid(), Description = "Mine #1", UserId = userId },
            new TaskItem { Id = Guid.NewGuid(), Description = "Mine #2", UserId = userId },
            new TaskItem { Id = Guid.NewGuid(), Description = "Someone else's", UserId = "other-user" }
        };

        var repoMock = new Mock<IRepository<TaskItem>>();
        repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(allTasks);

        var service = new GetTasksByUserService(
            repository: repoMock.Object,
            logger: NullLogger<GetTasksByUserService>.Instance
        );

        var claims = new ClaimsPrincipal(new ClaimsIdentity(
            new[] { new Claim(ClaimTypes.NameIdentifier, userId) }, "mock"));

        // Act
        var result = await TaskEndPoints.GetMyTasks(
            service,
            claims,
            CancellationToken.None);

        // Assert
        result.Should().BeOfType<Ok<IEnumerable<TaskItem>>>();

        var ok = (Ok<IEnumerable<TaskItem>>)result;
        ok.Value.Should().NotBeNull();
        ok.Value!.Should().OnlyContain(t => t.UserId == userId);
        ok.Value!.Should().HaveCount(2);
    }
    [Fact]
    public async Task GetTaskById_ShouldReturnOk_WithTask()
    {
        var repoMock = new Mock<IRepository<TaskItem>>();
        var taskId = Guid.NewGuid();
        var expectedTask = new TaskItem { Id = taskId, Description = "Test" };

        repoMock.Setup(r => r.GetByIdAsync(taskId.ToString(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTask);

        var logger = new Mock<ILogger<GetTaskByIdService>>().Object;

        var service = new GetTaskByIdService(repoMock.Object, logger);

        var result = await TaskEndPoints.GetTaskById(
            service, taskId, CancellationToken.None);

        result.Should().BeOfType<Ok<TaskItem>>();
    }

    [Fact]
    public async Task CreateTask_ShouldCallRepo_AndReturnCreated()
    {
        var repoMock = new Mock<IRepository<TaskItem>>();
        var request = new CreateTaskRequest
        {
            Description = "New task",
            DueDate = DateTime.UtcNow
        };

        var claims = new ClaimsPrincipal(new ClaimsIdentity(
            new[] { new Claim(ClaimTypes.NameIdentifier, "user-123") }, "mock"));

        var logger = new Mock<ILogger<CreateTaskService>>().Object;

        var service = new CreateTaskService(repoMock.Object, logger);

        var result = await TaskEndPoints.CreateTask(service, request, claims, CancellationToken.None);

        repoMock.Verify(r => r.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Once);
        result.Should().BeOfType<Created>();
    }

    [Fact]
    public async Task CompleteTask_ShouldCallRepo_AndReturnOk()
    {
        var repoMock = new Mock<IRepository<TaskItem>>();
        var taskId = Guid.NewGuid();
        var expectedTask = new TaskItem { Id = taskId, Description = "Test" };

        repoMock.Setup(r => r.GetByIdAsync(taskId.ToString(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTask);

        var logger = new Mock<ILogger<CompleteTaskService>>().Object;

        var service = new CompleteTaskService(repoMock.Object, logger);

        var result = await TaskEndPoints.CompleteTask(
            service, taskId, CancellationToken.None);

        repoMock.Verify(r => r.UpdateAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Once);
        result.Should().BeOfType<Ok>();
    }

    [Fact]
    public async Task DeleteTask_ShouldCallRepoDelete_WhenGivenId()
    {
        // Arrange
        var repoMock = new Mock<IRepository<TaskItem>>();
        var id = Guid.NewGuid();

        repoMock
            .Setup(r => r.Delete(id.ToString(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var logger = new Mock<ILogger<DeleteTaskService>>().Object;

        var service = new DeleteTaskService(repoMock.Object, logger);

        // Act
        var result = await TaskEndPoints.DeleteTask(service, id, CancellationToken.None);

        // Assert
        repoMock.Verify(r => r.Delete(id.ToString(), It.IsAny<CancellationToken>()), Times.Once);
        result.Should().BeOfType<NoContent>();
    }

    [Fact]
    public async Task ShouldThrow_UserNotFoundException_WhenUserIdIsEmpty()
    {
        // Arrange
        var repoMock = new Mock<IRepository<TaskItem>>();
        var loggerMock = new Mock<ILogger<GetTasksByUserService>>();

        var service = new GetTasksByUserService(repoMock.Object, loggerMock.Object);


        var claims = new ClaimsPrincipal(new ClaimsIdentity(
            new[] { new Claim(ClaimTypes.NameIdentifier, "") }, "mock"));

        // Act
        Func<Task> act = async () =>
            await TaskEndPoints.GetMyTasks(service, claims, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>();
    }

    [Fact]
    public async Task ShouldThrow_NoTasksFoundForUserException_WhenNoTasksExist()
    {
        // Arrange
        // Arrange
        var repoMock = new Mock<IRepository<TaskItem>>();
        var loggerMock = new Mock<ILogger<GetTasksByUserService>>();

        var service = new GetTasksByUserService(repoMock.Object, loggerMock.Object);

        var claims = new ClaimsPrincipal(new ClaimsIdentity(
            new[] { new Claim(ClaimTypes.NameIdentifier, "user-123") }, "mock"));

        // Act
        Func<Task> act = async () =>
            await TaskEndPoints.GetMyTasks(service, claims, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NoTasksFoundForUserException>();
    }


}