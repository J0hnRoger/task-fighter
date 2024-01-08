using MediatR;
using TaskFighter.Infrastructure.Persistence;

namespace TaskFighter.Domain.TasksManagement.Requests;

public record ListTaskRequest : IRequest<List<TodoTask>>
{
    public string Filters { get; set; }

    public ListTaskRequest(string value, string filters)
    {
        Filters = value;
    }

    public override string ToString()
    {
        return $"Query: List all Tasks";
    }
}

public class ListTaskCommandHandler : IRequestHandler<ListTaskRequest, List<TodoTask>>
{
    private readonly IFighterTaskContext _context;

    public ListTaskCommandHandler(IFighterTaskContext context)
    {
        _context = context;
    }

    public Task<List<TodoTask>> Handle(ListTaskRequest request, CancellationToken cancellationToken)
    {
        switch (request.Filters)
        {
            
            case "backlog":
                return Task.FromResult(_context.Backlog.ToList());
            case "all":
                return Task.FromResult(_context.Tasks.ToList());
            default:
                return Task.FromResult(_context.Tasks.Where(t => t.Status != TodoTaskStatus.Complete)
                    .ToList());
        }
    }
}