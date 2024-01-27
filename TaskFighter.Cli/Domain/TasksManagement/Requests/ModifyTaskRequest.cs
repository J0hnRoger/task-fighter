using MediatR;
using TaskFighter.Infrastructure.CommandParsing;
using TaskFighter.Infrastructure.Persistence;

namespace TaskFighter.Domain.TasksManagement.Requests;

public record ModifyTaskRequest : IRequest<TodoTask>
{
    public List<string> Tags { get; set; }
    public DateTime? Target { get; set; }
    public bool TargetBacklog { get; set; }
    public int Id { get; set; }

    public ModifyTaskRequest(string serializedValue, Filters filters)
    {
        Tags = filters.Tags;
        Target = filters.SpecialTags.Contains("1")
            ? DateTime.Today.AddDays(1)
            : filters.SpecialTags.Contains("0")
                ? DateTime.Today
                : null;
        
        if (filters.SpecialTags.Contains("B"))
            TargetBacklog = true;     
        
        if (!filters.Id.HasValue)
            throw new ArgumentException("Id is required to modify a task for the moment");

        Id = filters.Id.Value;
    }
}

public class ModifyTaskCommandHandler : IRequestHandler<ModifyTaskRequest, TodoTask>
{
    private readonly IFighterTaskContext _context;

    public ModifyTaskCommandHandler(IFighterTaskContext context)
    {
        _context = context;
    }

    public Task<TodoTask> Handle(ModifyTaskRequest request, CancellationToken cancellationToken)
    {
        var updatingTask = _context.Backlog.FirstOrDefault(t => t.Id == request.Id);
        DailyTodo? targetTodoList = null;
        if (updatingTask == null)
        {
            var todoListResult =_context.DailyTodoLists.GetDailyTodoWithTask(request.Id);
            if (todoListResult.IsFailure)
                throw new Exception($"No task found with id {request.Id} in backlog or daily todo list");
            targetTodoList = todoListResult.Value;
            updatingTask = todoListResult.Value.Tasks.First(t => t.Id == request.Id);
        }

        if (request.TargetBacklog)
        {
            if (targetTodoList != null)
                targetTodoList.Tasks.Remove(updatingTask);
            _context.BackToBacklog(updatingTask);
        } 
        else if (request.Target.HasValue)
        {
            var todoList = _context.DailyTodoLists.GetTodoList(request.Target.Value);
            if (todoList == null)
                throw new Exception($"No daily todo found for this date {request.Target:d}");

            if (todoList.Date == DateTime.Today && todoList.Opened)
                updatingTask.UnPlanned = true;

            updatingTask.Status = TodoTaskStatus.Planned;
            _context.AddToTodoList(updatingTask, todoList);
        }

        _context.SaveChanges();
        return Task.FromResult(updatingTask);
    }
}