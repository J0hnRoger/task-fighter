using MediatR;
using TaskSamurai.Domain.DayReporting;

namespace TaskSamurai.Domain.DayScheduling.Requests;
    
public record StartDayScheduleRequest : IRequest<Unit>
{
    public int EnergyLevel { get; set; }
    public int FocusLevel { get; set; }
    public string Notes { get; set; }
    
    public StartDayScheduleRequest(string serializedValue, string serializedFilters)
    {
        EnergyLevel = int.Parse(serializedValue.Split("energy:")[1]);
        Notes = serializedValue.Split("energy:")[0].Trim();
    }
}

public class StartDayScheduleRequestHandler : IRequestHandler<StartDayScheduleRequest, Unit>
{
    private readonly ISamuraiTaskContext _context;

    public StartDayScheduleRequestHandler(ISamuraiTaskContext context)
    {
        _context = context;
    }

    public Task<Unit> Handle(StartDayScheduleRequest request, CancellationToken cancellationToken)
    {
        _context.RaiseDomainEvent(DayEvents.StartDayEvent());
        _context.SaveChanges();
        
        return Task.FromResult(new Unit());
    }
}
