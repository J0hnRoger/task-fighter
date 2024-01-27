using MediatR;
using Spectre.Console;
using TaskFighter.Domain.TasksManagement;
using TaskFighter.Infrastructure.CommandParsing;

namespace TaskFighter.Domain.DayScheduling.Requests;

public record EndDayRequest : IRequest<Unit>
{
    public int? TodoListId { get; set; }

    public EndDayRequest(string values, Filters filters)
    {
        if (int.TryParse(filters.Raw, out int todoListId))
            TodoListId = todoListId;
    }
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
        DailyTodo currentTodoList =(request.TodoListId.HasValue)
            ? _context.DailyTodoLists.GetTodoList(request.TodoListId.Value) ?? _context.DailyTodo
            : _context.DailyTodo;

        if (currentTodoList.IsClosed)
        {
            AnsiConsole.WriteLine("You already closed your day. Go rest for today.");
            return Task.FromResult(new Unit());
        }

        var notFinishedTasks = currentTodoList.GetNotFinishedTasks();
        if (notFinishedTasks.Count == 0)
        {
            AnsiConsole.WriteLine("It was a good day, you finished all your tasks");
            currentTodoList.Shutdown(DateTime.Now);
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
                    _context.DeleteFromDailyTodo(task, currentTodoList.Id);
                    break;
                case "m":
                    _context.Migrate(task);
                    break;
                case "c":
                    task.Finish(DateTime.Now, force:true);
                    _context.Update(task);
                    _context.CompleteTask(task);
                    break;
                case "b":
                    _context.BackToBacklog(task);
                    break;
            }
        }

        currentTodoList.Shutdown(DateTime.Now);
        _context.SaveChanges();

        return Task.FromResult(new Unit());
    }

    private string DisplayChoices(TodoTask task)
    {
        DisplayTask(task);
        var actionChoice = AnsiConsole.Prompt(
            new TextPrompt<string>("What do we do with this task? (M)igrate | (B)acklog | (D)elete) | (C)omplete")
                .PromptStyle("green")
                .Validate(x => (new List<string>() {"d", "m", "b", "c"})
                    .Contains(x.ToLower())));

        return actionChoice;
    }

    private void DisplayTask(TodoTask task)
    {
        var panel = new Panel(task.Name);
        panel.Header = new PanelHeader(string.Join(",", task.Tags));
        panel.BorderStyle = new Style(Color.LightGoldenrod2_1);
        panel.Border = BoxBorder.Rounded;
        AnsiConsole.Write(panel);
    }
}