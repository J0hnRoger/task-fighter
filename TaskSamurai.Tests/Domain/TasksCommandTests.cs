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
    public async Task FinishTask_CreateFinishTaskEvent()
    {
        SamuraiTasksContext samuraiTasksContext = TestHelpers.CreateTestContext();
        var handler = new CreateTaskCommandHandler(samuraiTasksContext);
        
        var result = await handler.Handle(new AddTaskRequest("Test Task", "a:test")
            , CancellationToken.None);

        var startTaskHandler = new StartTaskRequestHandler(samuraiTasksContext);
        var startResult = startTaskHandler.Handle(new StartTaskRequest("", "1"), CancellationToken.None); 
        
        var endTaskHandler = new FinishTaskRequestHandler(samuraiTasksContext);
        var endResult = endTaskHandler.Handle(new FinishTaskRequest("", "1"), CancellationToken.None); 
        
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