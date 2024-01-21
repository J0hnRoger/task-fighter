using MediatR;
using TaskFighter.Infrastructure.CommandParsing;
using TaskFighter.Infrastructure.Persistence;

namespace TaskFighter.Domain.TasksManagement.Requests;

public record ListTaskRequest : IRequest<ListTodoTaskViewModel>
{
    public List<string> Tags { get; set; }
    public string Project { get; set; }

    public ListTaskRequest(string value, Filters filters)
    {
        // if (filters.StartsWith("p:"))
        //     Project = filters.Split("p:")[1]; 
        
        Tags = filters.Tags;
    }

    public override string ToString()
    {
        return $"Query: List all Tasks";
    }
}

public class ListTaskCommandHandler : IRequestHandler<ListTaskRequest, ListTodoTaskViewModel>
{
    private readonly IFighterTaskContext _context;

    public ListTaskCommandHandler(IFighterTaskContext context)
    {
        _context = context;
    }

    public Task<ListTodoTaskViewModel> Handle(ListTaskRequest request, CancellationToken cancellationToken)
    {
        List<TodoTask> tasks = new();
        switch (request.Project)
        {
            case "today":
                tasks = _context.Tasks.Where(t => t.Status != TodoTaskStatus.Complete)
                    .ToList();
                break;
            case "tomorrow":
                tasks = _context.Tomorrow.Tasks;
                break;
            case "all":
                tasks = _context.DailyTodo.Tasks;
                break;
            default:
                tasks = _context.Backlog.ToList();
                break;
        }

        var result = new ListTodoTaskViewModel() {Project = request.Project, Tasks = tasks};
        if (request.Project != "backlog")
        {
            result.Day = _context.DailyTodo.Date;
            result.IsOpened = _context.DailyTodo.Opened;
        }
        return  Task.FromResult(result);
    }
}

public class ListTodoTaskViewModel
{
    public string Project { get; set; }
    public List<TodoTask> Tasks { get; set; }
    public DateTime? Day { get; set; }
    public bool IsOpened { get; set; }
}