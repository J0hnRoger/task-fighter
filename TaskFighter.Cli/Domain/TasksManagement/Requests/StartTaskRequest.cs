using MediatR;
using TaskFighter.Infrastructure.CommandParsing;
using TaskFighter.Infrastructure.Persistence;

namespace TaskFighter.Domain.TasksManagement.Requests;

public class StartTaskRequest : IRequest<TodoTask>
{
    public int TaskId { get; set; }

    public StartTaskRequest(string value, Filters filter)
    {
        TaskId = int.Parse(filter.Raw);
    }

    public override string ToString()
    {
        return $"Command: Task Started: {TaskId}";
    }
}

public class StartTaskRequestHandler : IRequestHandler<StartTaskRequest, TodoTask>
{
    private readonly IFighterTaskContext _context;

    public StartTaskRequestHandler(IFighterTaskContext context)
    {
        _context = context;
    }

    public Task<TodoTask> Handle(StartTaskRequest request, CancellationToken cancellationToken)
    {
        var task = _context.DailyTodo.Tasks.Find(t => t.Id == request.TaskId);
        if (task == null)
            throw new Exception($"Task not found: {request.TaskId}");
        var alreadyActiveTask = _context.DailyTodo.Tasks.FirstOrDefault(t => t.Status == TodoTaskStatus.Active);
        if (alreadyActiveTask != null)
            throw new Exception($"Finish your current task first: {alreadyActiveTask}");
        
        task.Start(DateTime.Now);
        _context.Update(task);
        _context.SaveChanges();
        return Task.FromResult(task);
    }
}