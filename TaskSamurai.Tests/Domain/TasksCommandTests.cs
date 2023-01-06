using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using TaskSamurai.Domain.TasksManagement.Commands;
using TaskSamurai.Infrastructure.Persistence;
using Xunit;

namespace TaskFighter.Tests;

public class TasksCommandTests
{
    [Fact]
    public async Task CreateTask_CreateTask_WithMinimalMandatoryProperties()
    {
        SamuraiTasksContext samuraiTasksContext = TestHelpers.CreateTestContext();
        var handler = new CreateTaskCommandHandler(samuraiTasksContext);
        var result = await handler.Handle(new AddTaskRequest("Test Task", "a:test")
            , CancellationToken.None);

        result.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task ListTask_WithFilters_ReturnAdaptedView()
    {
        SamuraiTasksContext samuraiTasksContext = TestHelpers.CreateTestContext();
        var handler = new CreateTaskCommandHandler(samuraiTasksContext);
        var result = await handler.Handle(new AddTaskRequest("Test Task", "a:test")
            , CancellationToken.None);
        result.Id.Should().BeGreaterThan(0);
    }
}