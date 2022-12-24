using TaskSamurai.Domain.Common.Interfaces;

namespace TaskSamurai.Domain.DayScheduling;

public class TimeBlock : ITableRenderable
{
    public bool Done { get; private set; } = false;
    public TimeInterval Interval { get; set; }

    public bool IsDone(DateTime now)
    {
        return now > Interval.EndTime.GetAsDate(now);
    }

    public string[] GetFields()
    {
        return new string[]
        {
            Interval.ToString(),
            Done.ToString()
        };
    }
}