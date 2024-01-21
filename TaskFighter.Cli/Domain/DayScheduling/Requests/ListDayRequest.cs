using MediatR;
using Spectre.Console;
using TaskFighter.Domain.Common;
using TaskFighter.Domain.TasksManagement;
using TaskFighter.Infrastructure.CommandParsing;

namespace TaskFighter.Domain.DayScheduling.Requests;

public record ListDayRequest : IRequest<Unit>
{
    public DateTime From { get; set; } = DateTime.MinValue;

    public ListDayRequest(string values, Filters filters)
    {
        if (filters.Raw.Contains("week"))
            From = DateUtilities.GetFirstDayOfWeek();
    }
}

public class ListDayRequestHandler : IRequestHandler<ListDayRequest, Unit>
{
    private readonly IFighterTaskContext _context;

    public ListDayRequestHandler(IFighterTaskContext context)
    {
        _context = context;
    }

    public Task<Unit> Handle(ListDayRequest request, CancellationToken cancellationToken)
    {
        var openedTodoLists = _context.DailyTodoLists.GetOpenedTodoLists(request.From);

        if (!openedTodoLists.Any())
        {
            AnsiConsole.WriteLine("No Opened TodoLists");
            return Task.FromResult(new Unit());
        }

        DisplayTodoLists(openedTodoLists);

        return Task.FromResult(new Unit());
    }

    private void DisplayTodoLists(IEnumerable<DailyTodo> todoLists)
    {
        var tree = new Tree("Open Todo Lists");

        foreach (var list in todoLists)
        {
            var node = tree.AddNode($"[bold yellow]{list.Id}[/] {list.Date:d}");
            foreach (var task in list.Tasks)
            {
                node.AddNode(task.ToString());
            }
        }

        AnsiConsole.Write(tree);
    }
}