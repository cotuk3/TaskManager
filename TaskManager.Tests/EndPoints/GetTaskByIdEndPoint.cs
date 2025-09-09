using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Moq;
using TaskManager.Api.EndPoints.Tasks;
using TaskManager.Application.Tasks;
using TaskManager.Core.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions.DomainExceptions;

namespace TaskManager.Tests.EndPoints;
public class GetTaskByIdEndpointTests
{
    [Fact]
    public async Task ShouldReturnOk_WhenTaskExists()
    {
        var expectedTask = new TaskItem()
        {
            Id = Guid.NewGuid(),
            Description = "Test Task",
            UserId = "user-123"
        };
        var repoMock = new Mock<IRepository<TaskItem>>();
        var loggerMock = new Mock<ILogger<GetTaskByIdService>>();

        repoMock.Setup(r => r.GetByIdAsync(expectedTask.Id.ToString(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTask);

        var service = new GetTaskByIdService(repoMock.Object, loggerMock.Object);

        var result = await TaskEndPoints.GetTaskById(service, expectedTask.Id, CancellationToken.None);

        result.Should().BeOfType<Ok<TaskItem>>();
    }

    [Fact]
    public async Task ShouldThrow_TaskNotFoundException_WhenTaskDoesNotExist()
    {
        var repoMock = new Mock<IRepository<TaskItem>>();
        var loggerMock = new Mock<ILogger<GetTaskByIdService>>();


        repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TaskItem>());

        var service = new GetTaskByIdService(repoMock.Object, loggerMock.Object);

        Func<Task> act = async () => await TaskEndPoints.GetTaskById(service, Guid.NewGuid(), CancellationToken.None);

        await act.Should().ThrowAsync<TaskNotFoundException>();
    }

}