using MediatR;

namespace TaskSamurai.Domain.TasksManagement.Commands;

public class StartTaskRequest : IRequest<TodoTask>
{
    public int TaskId { get; set; }

    public StartTaskRequest(string value, string filter)
    {
        TaskId = int.Parse(filter);
    }

    public override string ToString()
    {
        return $"Command: Task Started: {TaskId}";
    }
}

public class StartTaskRequestHandler : IRequestHandler<StartTaskRequest, TodoTask>
{
    private readonly ISamuraiTaskContext _context;

    public StartTaskRequestHandler(ISamuraiTaskContext context)
    {
        _context = context;
    }

    public Task<TodoTask> Handle(StartTaskRequest request, CancellationToken cancellationToken)
    {
        var task = _context.GetTask(request.TaskId);
        task.Start(DateTime.Now);
        _context.Update(task);
        _context.SaveChanges();
        return Task.FromResult(task);
    }
}