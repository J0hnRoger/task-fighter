using TaskFighter.Domain.Common.Interfaces;
using TaskFighter.Domain.DayScheduling;

namespace TaskFighter.Models;

public class TimeBlockRenderer : ITableRenderable
{
    public TimeBlock TimeBlock { get; }

    public TimeBlockRenderer(TimeBlock timeBlock)
    {
        TimeBlock = timeBlock;
    }

    public string[] GetFieldValues(List<string> fields)
    {
            return new[]
            {
                TimeBlock.Interval.ToString(),
            };
    }
}