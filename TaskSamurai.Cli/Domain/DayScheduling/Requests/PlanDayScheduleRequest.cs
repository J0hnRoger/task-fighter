using MediatR;

namespace TaskSamurai.Domain.DayScheduling.Requests;

public record PlanDayScheduleRequest : IRequest<DaySchedule>
{
    public DateTime CurrentDay { get; set; }
    public PlanDayScheduleRequest(string serializedValue, string serializedFilters)
    {
    }
}

public class PlanDayScheduleCommandHandler : IRequestHandler<PlanDayScheduleRequest, DaySchedule>
{
    public Task<DaySchedule> Handle(PlanDayScheduleRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new DaySchedule()
        {
            TimeBlocks = new List<TimeBlock>()
            {
                new TimeBlock() {Interval = new TimeInterval() {StartTime = new Time(7, 0), EndTime = new Time(8, 30)}},
                new TimeBlock()
                    {Interval = new TimeInterval() {StartTime = new Time(8, 30), EndTime = new Time(10, 00)}},
                new TimeBlock()
                    {Interval = new TimeInterval() {StartTime = new Time(10, 00), EndTime = new Time(10, 30)}},
                new TimeBlock()
                    {Interval = new TimeInterval() {StartTime = new Time(10, 30), EndTime = new Time(12, 30)}},
                new TimeBlock()
                    {Interval = new TimeInterval() {StartTime = new Time(14, 00), EndTime = new Time(16, 00)}},
                new TimeBlock()
                    {Interval = new TimeInterval() {StartTime = new Time(16, 00), EndTime = new Time(17, 30)}},
                new TimeBlock()
                    {Interval = new TimeInterval() {StartTime = new Time(18, 00), EndTime = new Time(20, 00)}},
            }
        });
    }
}