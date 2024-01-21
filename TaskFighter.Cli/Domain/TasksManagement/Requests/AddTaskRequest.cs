using MediatR;
using TaskFighter.Infrastructure.CommandParsing;
using TaskFighter.Infrastructure.Persistence;

namespace TaskFighter.Domain.TasksManagement.Requests;
public record AddTaskRequest : IRequest<TodoTask>
{
    public List<string> Tags { get; set; }
    public string Context { get; set; }
    public string Name { get; set; }
    public string Area { get; set; }
    public string Project { get; set; }

    public AddTaskRequest(string serializedValue, Filters filters)
    {
        Tags = filters.Tags; 
        Name = serializedValue.Trim();

        Area = serializedValue.Contains("a:")
            ? serializedValue.Split("a:")[1].Split(" ")[0]
            : "work";

        Context = serializedValue.Contains("c:")
            ? serializedValue.Split("c:")[1].Split(" ")[0]
            : "perso";
        
        Project = serializedValue.Contains("p:") ? serializedValue.Split("p:")[1]
            .Split(" ")[0] 
            : "default";
    }
}

public class CreateTaskCommandHandler : IRequestHandler<AddTaskRequest, TodoTask>
{
    private readonly IFighterTaskContext _context;

    public CreateTaskCommandHandler(IFighterTaskContext context)
    {
        _context = context;
    }

    public Task<TodoTask> Handle(AddTaskRequest request, CancellationToken cancellationToken)
    {
        var task = new TodoTask()
        {
            Name = request.Name,
            Area = request.Area,
            Project = request.Project, 
            Context =  request.Context,
            Status = TodoTaskStatus.BackLog,
            Tags = request.Tags
        };

        if (!_context.DailyTodo.IsClosed)
            _context.AddTaskInDailyTodo(task);
        else
            _context.AddTask(task);
        _context.SaveChanges();
        return Task.FromResult(task);
    }
}