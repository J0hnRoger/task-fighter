using MediatR;
using Spectre.Console;
using TaskFighter.Domain.DayReporting;
using TaskFighter.Domain.TasksManagement;
using TaskFighter.Infrastructure.Persistence;

namespace TaskFighter.Domain.DayScheduling.Requests;
    
public record EndDayRequest : IRequest<Unit>
{
    public EndDayRequest(string values, string filters) { }
}

public class EndDayRequestHandler : IRequestHandler<EndDayRequest, Unit>
{
    private readonly IFighterTaskContext _context;

    public EndDayRequestHandler(IFighterTaskContext context)
    {
        _context = context;
    }

    public Task<Unit> Handle(EndDayRequest request, CancellationToken cancellationToken)
    {
        var notFinishedTasks = _context.DailyTodo.GetNotFinishedTasks();
        if (notFinishedTasks.Count == 0)
        {
            AnsiConsole.WriteLine("It was a good day, you finished all your tasks");
            _context.DailyTodo.Shutdown(DateTime.Now);
            _context.SaveChanges();
            return Task.FromResult(new Unit());
        }
        
        AnsiConsole.WriteLine("You have some unfinished tasks:");
        foreach (TodoTask task in notFinishedTasks)
        {
            var choice = DisplayChoices(task);
            switch (choice)
            {
                case "d":
                    _context.Delete(task);
                    break;
                case "m":
                    _context.Migrate(task);
                    break;
                case "b":
                    _context.BackToBacklog(task);
                    break;
            }
        }
        
        _context.DailyTodo.Shutdown(DateTime.Now);
        _context.SaveChanges();
        
        return Task.FromResult(new Unit());
    }

    private string DisplayChoices(TodoTask task)
    {
        DisplayTask(task);
        var actionChoice = AnsiConsole.Prompt(
            new TextPrompt<string>("What do we do with this task? (M)igrate | (B)acklog | (D)elete)")
                .PromptStyle("green")
                .Validate(x => (new List<string> () { "d", "m", "b" })
                    .Contains(x.ToLower())));
        
        return actionChoice;
    }

    private void DisplayTask(TodoTask task)
    {
        var panel = new Panel(task.Name);
        panel.Header = new PanelHeader(task.Project);
        panel.BorderStyle = new Style(Color.LightGoldenrod2_1);
        panel.Border = BoxBorder.Rounded;
        AnsiConsole.Write(panel);
    }
}
