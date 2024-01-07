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
    
    public string[] GetFields()
    {
            return new[]
            {
                TimeBlock.Interval.ToString(),
            };
    }
}