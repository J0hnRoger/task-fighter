using System;
using TaskSamurai.Domain.DayScheduling;
using Xunit;

namespace TaskFighter.Tests;

public class DayScheduleTests
{
    [Fact]
    public void DaySchedule_ContainsTimeBlocks()
    {
        DaySchedule dayWork = new DaySchedule();
        dayWork.TimeBlocks.Add(new TimeBlock()
        {
            Interval = new TimeInterval()
            {
                StartTime = new Time(8, 30),
                EndTime = new Time(10, 30),
            }
        });
        // Craft your day
        dayWork.StartDay(new DateTime(2020, 12, 1, 8, 30, 0));
    }
}