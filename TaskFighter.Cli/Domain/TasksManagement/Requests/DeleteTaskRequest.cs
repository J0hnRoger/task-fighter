using MediatR;
using TaskFighter.Infrastructure.CommandParsing;

namespace TaskFighter.Domain.TasksManagement.Requests;

public record DeleteTaskRequest : IRequest<Unit>
{
    public int TaskId { get; set; }

    public DeleteTaskRequest(string value, Filters filter)
    {
        if (filter.Id.HasValue)
            TaskId = filter.Id.Value;
    } 
    
    public override string ToString()
    {
        return $"Command: Delete Task Id {TaskId}";
    }
}

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskRequest, Unit>
{
    private readonly IFighterTaskContext _context;

    public DeleteTaskCommandHandler(IFighterTaskContext context)
    {
        _context = context;
    }

    public Task<Unit> Handle(DeleteTaskRequest request, CancellationToken cancellationToken)
    {
        _context.DeleteTask(request.TaskId);
        _context.SaveChanges();
        return Task.FromResult(new Unit());
    }
}