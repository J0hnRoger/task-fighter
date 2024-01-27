using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using TaskFighter.Domain.TasksManagement.Requests;
using TaskFighter.Infrastructure.CommandParsing;
using TaskFighter.Infrastructure.Persistence;
using Xunit;

namespace TaskFighter.Tests;

public class TasksCommandTests
{
    [Fact]
    public async Task CreateTask_WithMinimalMandatoryProperties()
    {
        FighterTasksContext fighterTasksContext = TestHelpers.CreateTestContext();
        var handler = new AddTaskCommandHandler(fighterTasksContext);
        var result = await handler.Handle(new AddTaskRequest("Test Task", new Filters("a:test"))
            , CancellationToken.None);

        result.Id.Should().BeGreaterThan(0);
    }
    
    [Fact]
    public async Task AddTask_WithModifiers()
    {
        FighterTasksContext fighterTasksContext = TestHelpers.CreateTestContext();
        var handler = new AddTaskCommandHandler(fighterTasksContext);
        var result = await handler.Handle(new AddTaskRequest("Test Task", new Filters("a:test"))
            , CancellationToken.None);

        result.Id.Should().BeGreaterThan(0);
    }
    
    [Fact]
    public async Task FinishTask_CreateFinishTaskEvent()
    {
        FighterTasksContext fighterTasksContext = TestHelpers.CreateTestContext();
        var handler = new AddTaskCommandHandler(fighterTasksContext);
        
        var result = await handler.Handle(new AddTaskRequest("Test Task", new Filters("a:test"))
            , CancellationToken.None);

        var startTaskHandler = new StartTaskRequestHandler(fighterTasksContext);
        var startResult = await startTaskHandler.Handle(new StartTaskRequest("", new Filters(result.Id.ToString())), CancellationToken.None); 
        
        var endTaskHandler = new DoneTaskRequestHandler(fighterTasksContext);
        var endResult = await endTaskHandler.Handle(new DoneTaskRequest("", new Filters(result.Id.ToString())), CancellationToken.None); 
        
        result.Id.Should().Be(1);
        result.Status.Should().Be(TodoTaskStatus.Complete);
    }

    [Fact]
    public async Task ListTask_WithFilters_ReturnAdaptedView()
    {
        FighterTasksContext fighterTasksContext = TestHelpers.CreateTestContext();
        var handler = new AddTaskCommandHandler(fighterTasksContext);
        var result = await handler.Handle(new AddTaskRequest("Test Task", new Filters("a:test"))
            , CancellationToken.None);
        result.Id.Should().BeGreaterThan(0);
    }
}