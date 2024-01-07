using FluentAssertions;
using TaskFighter.Domain.DayScheduling;
using Xunit;

namespace TaskFighter.Tests;

public class TimeBlockTests
{
    [Fact]
    public void TimeBlock_ReturnTheDuration()
    {
        TimeBlock timeBlock = new TimeBlock();
        timeBlock.Interval = new TimeInterval()
        {
            StartTime = new Time(8,15),
            EndTime =  new Time(10,0),
        };
        timeBlock.Interval.Duration.Should().Be(105);
    }
}