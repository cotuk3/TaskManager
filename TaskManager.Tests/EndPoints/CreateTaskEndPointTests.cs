using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Api.Endpoints.Tasks;
using TaskManager.Api.EndPoints.Tasks;
using TaskManager.Application.Tasks;
using TaskManager.Core.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions.DomainExceptions;
using TaskManager.Domain.Exceptions.InfrastractureExceptions;
using TaskManager.Tests.Common;

namespace TaskManager.Tests.EndPoints;
public class CreateTaskEndpointTests
{
    [Fact]
    public async Task ShouldReturnCreated_WhenTaskIsValid()
    {
        // Arrange
        var createTaskRequest = new CreateTaskRequest()
        {
            Description = "Description",
            DueDate = DateTime.UtcNow.AddDays(5)
        };

        var _repoMock = new Mock<IRepository<TaskItem>>();
        var _loggerMock = new Mock<ILogger<CreateTaskService>>();

        var service = new CreateTaskService(_repoMock.Object, _loggerMock.Object);

        var claims = TestClaimsFactory.Create("user-123");

        // Act
        var result = await TaskEndPoints.CreateTask(service, createTaskRequest, claims, CancellationToken.None);

        // Assert
        result.Should().BeOfType<Created>();
    }

    [Fact]
    public async Task ShouldThrow_InvalidDueDateException_WhenDueDateIsInPast()
    {
        // Arrange
        var createTaskRequest = new CreateTaskRequest()
        {
            Description = "Description",
            DueDate = DateTime.UtcNow.AddDays(-1)
        };

        var _repoMock = new Mock<IRepository<TaskItem>>();
        var _loggerMock = new Mock<ILogger<CreateTaskService>>();

        var service = new CreateTaskService(_repoMock.Object, _loggerMock.Object);

        var claims = TestClaimsFactory.Create("user-123");

        // Act
        Func<Task> act = async () =>
            await TaskEndPoints.CreateTask(service, createTaskRequest, claims, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidDueDateException>();
    }

    [Fact]
    public async Task ShouldThrow_InvalidDueDateException_WhenDueDateIsDistantFuture()
    {
        // Arrange
        var createTaskRequest = new CreateTaskRequest()
        {
            Description = "Description",
            DueDate = DateTime.UtcNow.AddYears(21)
        };

        var _repoMock = new Mock<IRepository<TaskItem>>();
        var _loggerMock = new Mock<ILogger<CreateTaskService>>();

        var service = new CreateTaskService(_repoMock.Object, _loggerMock.Object);

        var claims = TestClaimsFactory.Create("user-123");

        // Act
        Func<Task> act = async () =>
            await TaskEndPoints.CreateTask(service, createTaskRequest, claims, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidDueDateException>();
    }

}