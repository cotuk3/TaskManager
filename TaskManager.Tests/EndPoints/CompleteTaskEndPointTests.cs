using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Moq;
using TaskManager.Api.EndPoints.Tasks;
using TaskManager.Application.Tasks;
using TaskManager.Core.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions.DomainExceptions;
using Xunit.Sdk;

namespace TaskManager.Tests.EndPoints;
public class CompleteTaskEndpointTests
{
    [Fact]
    public async Task ShouldMarkTaskAsCompleted_WhenTaskIsValid()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var task = new TaskItem
        {
            Id = taskId,
            Description = "Test",
            IsCompleted = false
        };

        var repoMock = new Mock<IRepository<TaskItem>>();
        var loggerMock = new Mock<ILogger<CompleteTaskService>>();

        repoMock.Setup(r => r.GetByIdAsync(taskId.ToString(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(task);


        repoMock.Setup(r => r.UpdateAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

        var service = new CompleteTaskService(repoMock.Object, loggerMock.Object);

        // Act
        var result = await TaskEndPoints.CompleteTask(service, taskId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<Ok>();

        repoMock.Verify(r => r.UpdateAsync(
            It.Is<TaskItem>(t => t.IsCompleted && t.Id == taskId),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }


    [Fact]
    public async Task ShouldThrow_TaskNotFoundException_WhenTaskDoesNotExist()
    {
        // Arrange
        var repoMock = new Mock<IRepository<TaskItem>>();
        var loggerMock = new Mock<ILogger<CompleteTaskService>>();

        repoMock.Setup(r => r.GetByIdAsync("", It.IsAny<CancellationToken>()))
        .ReturnsAsync(new TaskItem()
        {
            Id = Guid.NewGuid(),
            Description = "Test",
            IsCompleted = false
        });

        var service = new CompleteTaskService(repoMock.Object, loggerMock.Object);

        // Act
        Func<Task> act = async () =>
            await TaskEndPoints.CompleteTask(service, Guid.NewGuid(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<TaskNotFoundException>();
    }

    [Fact]
    public async Task ShouldThrow_TaskAlreadyCompletedException_WhenTaskAlreadyCompleted()
    {
        // Arrange
        var repoMock = new Mock<IRepository<TaskItem>>();
        var loggerMock = new Mock<ILogger<CompleteTaskService>>();

        repoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(new TaskItem()
        {
            Id = Guid.NewGuid(),
            Description = "Test",
            IsCompleted = true
        });

        var service = new CompleteTaskService(repoMock.Object, loggerMock.Object);

        // Act
        Func<Task> act = async () =>
            await TaskEndPoints.CompleteTask(service, Guid.NewGuid(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<TaskAlreadyCompletedException>();
    }
}