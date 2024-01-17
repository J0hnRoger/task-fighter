using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using TaskFighter.Domain.TasksManagement.Requests;
using TaskFighter.Infrastructure.Persistence;
using Xunit;

namespace TaskFighter.Tests;

public class TasksCommandTests
{
    [Fact]
    public async Task CreateTask_WithMinimalMandatoryProperties()
    {
        FighterTasksContext fighterTasksContext = TestHelpers.CreateTestContext();
        var handler = new CreateTaskCommandHandler(fighterTasksContext);
        var result = await handler.Handle(new AddTaskRequest("Test Task", "a:test")
            , CancellationToken.None);

        result.Id.Should().BeGreaterThan(0);
    }
    
    [Fact]
    public async Task CreateTask_WithModifiers()
    {
        FighterTasksContext fighterTasksContext = TestHelpers.CreateTestContext();
        var handler = new CreateTaskCommandHandler(fighterTasksContext);
        var result = await handler.Handle(new AddTaskRequest("Test Task", "a:test")
            , CancellationToken.None);

        result.Id.Should().BeGreaterThan(0);
    }
    
    [Fact]
    public async Task FinishTask_CreateFinishTaskEvent()
    {
        FighterTasksContext fighterTasksContext = TestHelpers.CreateTestContext();
        var handler = new CreateTaskCommandHandler(fighterTasksContext);
        
        var result = await handler.Handle(new AddTaskRequest("Test Task", "a:test")
            , CancellationToken.None);

        var startTaskHandler = new StartTaskRequestHandler(fighterTasksContext);
        var startResult = await startTaskHandler.Handle(new StartTaskRequest("", result.Id.ToString()), CancellationToken.None); 
        
        var endTaskHandler = new DoneTaskRequestHandler(fighterTasksContext);
        var endResult = await endTaskHandler.Handle(new DoneTaskRequest("", result.Id.ToString()), CancellationToken.None); 
        
        result.Id.Should().Be(1);
        result.Status.Should().Be(TodoTaskStatus.Complete);
    }

    [Fact]
    public async Task ListTask_WithFilters_ReturnAdaptedView()
    {
        FighterTasksContext fighterTasksContext = TestHelpers.CreateTestContext();
        var handler = new CreateTaskCommandHandler(fighterTasksContext);
        var result = await handler.Handle(new AddTaskRequest("Test Task", "a:test")
            , CancellationToken.None);
        result.Id.Should().BeGreaterThan(0);
    }
}