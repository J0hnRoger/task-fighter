using MediatR;
using Spectre.Console;
using TaskFighter.Domain.TasksManagement;
using TaskFighter.Infrastructure.CommandParsing;
using TaskFighter.Infrastructure.Renderer;

namespace TaskFighter.Domain.DayScheduling.Requests;

public record ShowDayRequest : IRequest<Unit>
{
    public ShowDayRequest(string values, Filters filters)
    { }
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
        DisplayTodoList(_context.DailyTodo);
        return Task.FromResult(new Unit());
    }

    private void DisplayTodoList(DailyTodo dailyTodo)
    {
        TaskFighterTable<TodoTask> table = new(dailyTodo.Tasks, new List<string>()
        {
           "Id", "Name", "Status", "Project" , "Tags"
        });
        AnsiConsole.Write(table.Table);
    }
}