using System.Globalization;

namespace TaskFighter.Domain.Common;

public class DateUtilities
{
  public static DateTime GetFirstDayOfWeek()
    {
        DateTime now = DateTime.Now;
        DayOfWeek currentDay = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
        int daysToSubtract = (now.DayOfWeek - currentDay + 7) % 7;
        DateTime firstDayOfWeek = now.AddDays(-daysToSubtract);

        return firstDayOfWeek;
    }}