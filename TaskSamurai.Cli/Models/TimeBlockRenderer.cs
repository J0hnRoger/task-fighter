using TaskSamurai.Domain.Common.Interfaces;
using TaskSamurai.Domain.DayScheduling;

namespace TaskSamurai.Models;

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