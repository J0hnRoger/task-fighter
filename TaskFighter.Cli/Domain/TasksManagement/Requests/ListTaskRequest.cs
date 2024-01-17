using MediatR;
using TaskFighter.Infrastructure.Persistence;

namespace TaskFighter.Domain.TasksManagement.Requests;

public record ListTaskRequest : IRequest<ListTodoTaskViewModel>
{
    public string Filters { get; set; }
    public string Project { get; set; }

    public ListTaskRequest(string value, string filters)
    {
        if (filters.StartsWith("p:"))
            Project = filters.Split("p:")[1]; 
            
        Filters = filters;
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
            case "backlog":
                tasks = _context.Backlog.ToList();
                break;
            case "tomorrow":
                tasks = _context.Tomorrow.Tasks;
                break;
            default:
                tasks = _context.Tasks.Where(t => t.Status != TodoTaskStatus.Complete)
                    .ToList();
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