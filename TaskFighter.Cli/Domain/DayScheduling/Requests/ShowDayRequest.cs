using MediatR;
using Spectre.Console;
using TaskFighter.Domain.TasksManagement;
using TaskFighter.Infrastructure.CommandParsing;
using TaskFighter.Infrastructure.Renderer;

namespace TaskFighter.Domain.DayScheduling.Requests;

public record ShowDayRequest : IRequest<Unit>
{
    public DateTime Date { get; set; } 
    
    public ShowDayRequest(string values, Filters filters)
    {
       // Special case for tomorrow 
       if (filters.Tags.Contains("1")) 
           Date = DateTime.Today.AddDays(1);
       else 
           Date = DateTime.Today;
    }
}

public class ShowDayRequestHandler : IRequestHandler<ShowDayRequest, Unit>
{
    private readonly IFighterTaskContext _context;

    public ShowDayRequestHandler(IFighterTaskContext context)
    {
        _context = context;
    }

    public Task<Unit> Handle(ShowDayRequest request, CancellationToken cancellationToken)
    {
        var dailyTodo = _context.DailyTodoLists.GetTodoList(request.Date);
        if (dailyTodo == null)
            throw new Exception($"No daily todo found for this date {request.Date}");
        
        DisplayTodoList(dailyTodo);
        return Task.FromResult(new Unit());
    }

    private void DisplayTodoList(DailyTodo dailyTodo)
    {
        AnsiConsole.Write(new Markup($"[cadetblue]{dailyTodo.Date:d} - Status: {(dailyTodo.Opened ? "Opened" : "Closed")}[/]\n"));
        TaskFighterTable<TodoTask> table = new(dailyTodo.Tasks, new List<string>()
        {
           "Id", "Name", "Status", "Project" , "Tags"
        });
        AnsiConsole.Write(table.Table);
    }
}