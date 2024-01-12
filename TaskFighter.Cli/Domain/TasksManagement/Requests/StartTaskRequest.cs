using MediatR;

namespace TaskFighter.Domain.TasksManagement.Requests;

public class StartTaskRequest : IRequest<TodoTask>
{
    public int TaskId { get; set; }

    public StartTaskRequest(string value, string filter)
    {
        TaskId = int.Parse(value);
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
        task.Start(DateTime.Now);
        _context.Update(task);
        _context.SaveChanges();
        return Task.FromResult(task);
    }
}