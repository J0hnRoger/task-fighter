using MediatR;
using TaskSamurai.Domain.DayReporting;

namespace TaskSamurai.Domain.DayScheduling.Requests;
    
public record EndDayScheduleRequest : IRequest<Unit>
{
    public int EnergyLevel { get; set; }
    public int FocusLevel { get; set; }
    public string Notes { get; set; }
    
    public EndDayScheduleRequest(string serializedValue, string serializedFilters)
    {
        EnergyLevel = int.Parse(serializedValue.Split("energy:")[1]);
        Notes = serializedValue.Split("energy:")[0].Trim();
    }
}

public class EndDayScheduleRequestHandler : IRequestHandler<EndDayScheduleRequest, Unit>
{
    private readonly ISamuraiTaskContext _context;

    public EndDayScheduleRequestHandler(ISamuraiTaskContext context)
    {
        _context = context;
    }

    public Task<Unit> Handle(EndDayScheduleRequest request, CancellationToken cancellationToken)
    {
        // TODO - var currentDay = _context.GetCurrentDaySchedule()
        //  var currentDay.End() <-- update les Task non-terminées
        // report.CalculateDuration();

        var report = new DailyReport(request.Notes, request.EnergyLevel, DateTime.Now);

        _context.RaiseDomainEvent(DayEvents.EndDayEvent(report));
        _context.SaveChanges();
        
        return Task.FromResult(new Unit());
    }
}
