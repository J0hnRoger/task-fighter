using MediatR;
using TaskFighter.Infrastructure.CommandParsing;

namespace TaskFighter.Domain.EventsManagement.Requests;

public class UpdateEventRequest : IRequest<TodoEvent>
{

    public UpdateEventRequest(string value, Filters filter)
    {
    }

    public override string ToString()
    {
        return $"Command: Event Updated:";
    }
}

public class UpdateEventRequestHandler : IRequestHandler<UpdateEventRequest, TodoEvent>
{
    private readonly IFighterTaskContext _context;

    public UpdateEventRequestHandler(IFighterTaskContext context)
    {
        _context = context;
    }

    public Task<TodoEvent> Handle(UpdateEventRequest request, CancellationToken cancellationToken)
    {
        _context.SaveChanges();
        return Task.FromResult(new TodoEvent());
    }
}