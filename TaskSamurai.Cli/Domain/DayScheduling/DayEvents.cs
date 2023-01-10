﻿using TaskSamurai.Domain.Common;

namespace TaskSamurai.Domain.DayReporting;

public class DayEvents
{
    public static BaseEvent<Entity> EndDayEvent(DailyReport report)
    {
        return new DayEndedEvent(report, DateTime.Now);
    }
}
public class DayEndedEvent : BaseEvent<Entity>
{
    public DayEndedEvent(DailyReport report, DateTime created) : base("DayEndedEvent", created, report) { }
}
