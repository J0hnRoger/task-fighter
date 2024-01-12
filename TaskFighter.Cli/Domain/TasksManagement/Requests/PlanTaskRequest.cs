using MediatR;
using Spectre.Console;

namespace TaskFighter.Domain.TasksManagement.Requests;

public record PlanTaskRequest : IRequest<List<TodoTask>>
{
    public PlanTaskRequest(string value, string filters)
    {
    }
}

public class PlanTaskCommandHandler : IRequestHandler<PlanTaskRequest, List<TodoTask>>
{
    private readonly IFighterTaskContext _context;

    public PlanTaskCommandHandler(IFighterTaskContext context)
    {
        _context = context;
    }

    public Task<List<TodoTask>> Handle(PlanTaskRequest request, CancellationToken cancellationToken)
    {
        if (_context.DailyTodoLists.HasOpenedDay)
        {
            throw new Exception("Opened day, close it before planning a new one");
        }
        
        var allTasks = _context.Backlog.ToList();
        var dailyTasks = DisplayTasksAsChoices(allTasks);
        
        foreach (TodoTask task in dailyTasks)
            _context.TackleToday(task);
        _context.SaveChanges();
        
        return Task.FromResult(allTasks);
    }

    private List<TodoTask> DisplayTasksAsChoices(List<TodoTask> allTasks)
    {
        var todoTodayTasks = AnsiConsole.Prompt(
            new MultiSelectionPrompt<TodoTask>()
                .Title("It's a great time for tackle some tasks")
                .PageSize(10)
                .MoreChoicesText("Select 1 to 4 tasks")
                .InstructionsText(
                    "[grey](Press [blue]<space>[/] to select a task, " +
                    "[green]<enter>[/] to validate the day)[/]")
                .AddChoices(allTasks));
        
        return todoTodayTasks;
    }
}