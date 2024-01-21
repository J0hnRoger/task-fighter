using MediatR;
using TaskFighter.Domain.TasksManagement;
using TaskFighter.Infrastructure.CommandParsing;
using TaskFighter.Infrastructure.Persistence;

namespace TaskFighter.Domain.DayScheduling.Requests;

public record PlanDayScheduleRequest : IRequest<DaySchedule>
{
    public DateTime CurrentDay { get; set; }

    public PlanDayScheduleRequest(string serializedValue, Filters filters)
    {
    }
}

public class PlanDayScheduleRequestHandler : IRequestHandler<PlanDayScheduleRequest, DaySchedule>
{
    private readonly IFighterTaskContext _context;

    public PlanDayScheduleRequestHandler(IFighterTaskContext context)
    {
        _context = context;
    }

    public Task<DaySchedule> Handle(PlanDayScheduleRequest request, CancellationToken cancellationToken)
    {
        DaySchedule current = DaySchedule.CreateWorkDay();
        List<TodoTask> todos = _context.Tasks.Where(t => t.Status == TodoTaskStatus.BackLog)
            .ToList();
        return Task.FromResult(current);
    }
}