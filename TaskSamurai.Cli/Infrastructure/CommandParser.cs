using MediatR;
using TaskSamurai.Domain.DayScheduling.Commands;

namespace TaskSamurai.Infrastructure;

public class CommandParser
{
    public IBaseRequest ParseArgs(string[] args)
    {
        return new GenerateDayScheduleCommand{ CurrentDay = DateTime.Now };
    }
}