using MediatR;
using TaskFighter.Infrastructure.CommandParsing;
using TaskFighter.Infrastructure.Persistence;

namespace TaskFighter.Domain.TasksManagement.Requests;

public record AddTaskRequest : IRequest<TodoTask>
{
    public List<string> Tags { get; set; }
    public DateTime? Target { get; set; }
    public string Context { get; set; }
    public string Name { get; set; }

    public AddTaskRequest(string serializedValue, Filters filters)
    {
        Tags = filters.Tags;
        Target = filters.SpecialTags.Contains("1")
            ? DateTime.Today.AddDays(1)
            : filters.SpecialTags.Contains("0")
                ? DateTime.Today
                : null;

        Name = serializedValue.Trim();
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
            Context = request.Context,
            Status = TodoTaskStatus.BackLog,
            Tags = request.Tags
        };

        if (request.Target.HasValue)
        {
            var todoList = _context.DailyTodoLists.GetTodoList(request.Target.Value);
            if (todoList == null)
                throw new Exception($"No daily todo found for this date {request.Target:d}");
            
            if (todoList.Date == DateTime.Today && todoList.Opened)
                task.UnPlanned = true;
                    
            task.Status = TodoTaskStatus.Planned;
            _context.AddTask(task, todoList);
        }
        else
        {
            _context.AddTask(task);
        }
        _context.SaveChanges();
        return Task.FromResult(task);
    }
}