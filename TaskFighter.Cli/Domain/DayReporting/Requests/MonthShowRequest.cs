using MediatR;
using Spectre.Console;
using TaskFighter.Domain.TasksManagement;
using TaskFighter.Infrastructure.CommandParsing;
using TaskFighter.Infrastructure.Persistence;

namespace TaskFighter.Domain.DayReporting.Requests;

public record ShowReportRequest : IRequest
{
    public ShowReportRequest(string value, Filters filters)
    {
    }
}

public class ShowReportRequestHandler : IRequestHandler<ShowReportRequest>
{
    private readonly IFighterTaskContext _context;

    public ShowReportRequestHandler(IFighterTaskContext context)
    {
        _context = context;
    }

    public Task<Unit> Handle(ShowReportRequest request, CancellationToken cancellationToken)
    {
        DateTime now = DateTime.Now;
        DateTime startOfMonth = new DateTime(now.Year, now.Month, 1);
        var allTasks = _context.DailyTodoLists.Days.SelectMany(d => d.Tasks)
            .Where(t => t.EndDate > startOfMonth) 
            .ToList();
        
        DisplayMonth(startOfMonth, allTasks);
        
        return Task.FromResult(new Unit());
    }

    private void DisplayMonth(DateTime startMonth, List<TodoTask> tasks)
    {
        var calendar = new Calendar(startMonth);
        foreach (TodoTask todoTask in tasks)
        {
            calendar.AddCalendarEvent(todoTask.EndDate);
        } 
        calendar.HighlightStyle(Style.Parse("yellow bold"));
        AnsiConsole.Write(calendar);
    }
}