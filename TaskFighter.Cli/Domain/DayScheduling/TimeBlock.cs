using TaskFighter.Domain.Common.Interfaces;
using TaskFighter.Domain.TasksManagement;

namespace TaskFighter.Domain.DayScheduling;

public class TimeBlock : ITableRenderable
{
    public bool Done { get; private set; } = false;
    public TodoTask? Task { get; private set; }
    public TimeInterval Interval { get; set; }

    public bool Planned => Task != null;
    
    public bool IsDone(DateTime now)
    {
        return now > Interval.EndTime.GetAsDate(now);
    }

    public void SetTask(TodoTask task)
    {
        Task = task;
    }

    public string[] GetFieldValues(List<string> fields)
    {
        return new string[]
        {
            Interval.ToString(),
            Done.ToString()
        };
    }
}