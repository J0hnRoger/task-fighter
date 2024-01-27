using MediatR;
using TaskFighter.Infrastructure.CommandParsing;
using TaskFighter.Infrastructure.Persistence;

namespace TaskFighter.Domain.TasksManagement.Requests;

public record CancelTaskRequest : IRequest<TodoTask>
{
    public int Id { get; set; }

    public CancelTaskRequest(string serializedValue, Filters filters)
    {
        Id = int.Parse(filters.Raw);
    }
}

public class CancelTaskCommandHandler : IRequestHandler<CancelTaskRequest, TodoTask>
{
    private readonly IFighterTaskContext _context;

    public CancelTaskCommandHandler(IFighterTaskContext context)
    {
        _context = context;
    }

    public Task<TodoTask> Handle(CancelTaskRequest request, CancellationToken cancellationToken)
    {
        if (_context.DailyTodo.IsClosed)
            throw new Exception("Daily todo is closed, you can't cancel a task.");

        var task = _context.DailyTodo.Tasks.Find(t => t.Id == request.Id);
        if (task == null)
            throw new Exception($"Task not found: {request.Id}");
        task.Status = TodoTaskStatus.Planned;
        _context.Update(task);
        _context.SaveChanges();
        return Task.FromResult(task);
    }
}