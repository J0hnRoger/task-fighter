using MediatR;
using TaskFighter.Infrastructure.CommandParsing;

namespace TaskFighter.Domain.EventsManagement.Requests;

public record ListEventRequest : IRequest<List<TodoEvent>>
{
    public string Filters { get; set; }

    public ListEventRequest(string value, Filters filters)
    {
    }

    public override string ToString()
    {
        return $"Query: List all Events";
    }
}

public class ListEventCommandHandler : IRequestHandler<ListEventRequest, List<TodoEvent>>
{
    private readonly IFighterTaskContext _context;

    public ListEventCommandHandler(IFighterTaskContext context)
    {
        _context = context;
    }

    public Task<List<TodoEvent>> Handle(ListEventRequest request, CancellationToken cancellationToken)
    {
        switch (request.Filters)
        {
            default:
                return Task.FromResult(_context.Events.ToList());
        }
    }
}