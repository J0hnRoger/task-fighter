using CSharpFunctionalExtensions;

namespace TaskSamurai.Domain.DayScheduling;

public class Time : ValueObject
{
    public int Hour { get; set; }
    public int Minutes { get; set; }
    public int Duration => Hour * 60 + Minutes;

    public DateTime GetAsDate(DateTime now)
    {
        DateTime endDate = new DateTime(now.Year, now.Month, now.Day, Hour, Minutes, 0);
        return endDate;
    }
    public Time(int hour, int minutes)
    {
        if (hour > 23)
            throw new Exception("Les heures ne peuvent excéder 23");
        if (minutes > 59)
            throw new Exception("Les minutes ne peuvent excéder 60");
        Hour = hour;
        Minutes = minutes;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Hour;
        yield return Minutes;
    }
}