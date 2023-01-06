using CSharpFunctionalExtensions;

namespace TaskSamurai.Domain.DayScheduling;

public class TimeInterval : ValueObject
{
    public Time StartTime { get; set; }
    public Time EndTime { get; set; }

    public int Duration => (EndTime.Duration - StartTime.Duration);
    
    public override string ToString()
    {
        return $"|-- {StartTime} --| \r\n" +
               $"|-- {EndTime} --|";
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return StartTime;
        yield return EndTime;
    }

    public bool Include(DateTime now)
    {
        DateTime startDate = StartTime.GetAsDate(now);
        DateTime endDate = EndTime.GetAsDate(now);
        return now >= startDate && now <= endDate;
    }
}