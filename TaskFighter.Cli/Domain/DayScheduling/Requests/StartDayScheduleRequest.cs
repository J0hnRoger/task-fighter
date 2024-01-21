using MediatR;
using TaskFighter.Infrastructure.CommandParsing;

namespace TaskFighter.Domain.DayScheduling.Requests;
    
public record StartDayScheduleRequest : IRequest<Unit>
{
    public StartDayScheduleRequest(string serializedValue, Filters filters)
    { }
}

public class StartDayScheduleRequestHandler : IRequestHandler<StartDayScheduleRequest, Unit>
{
    private readonly IFighterTaskContext _context;

    public StartDayScheduleRequestHandler(IFighterTaskContext context)
    {
        _context = context;
    }

    public Task<Unit> Handle(StartDayScheduleRequest request, CancellationToken cancellationToken)
    {
        // TODO - get planned day ID
        _context.RaiseDomainEvent(DayEvents.StartDayEvent());
        _context.SaveChanges();
        
        return Task.FromResult(new Unit());
    }
}
