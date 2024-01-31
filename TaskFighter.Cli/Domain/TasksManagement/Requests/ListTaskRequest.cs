using MediatR;
using Spectre.Console;
using TaskFighter.Infrastructure.CommandParsing;
using TaskFighter.Infrastructure.Renderer;

namespace TaskFighter.Domain.TasksManagement.Requests;

public record ListTaskRequest : IRequest
{
    public List<string> Tags { get; set; }
    public string Project { get; set; }

    public ListTaskRequest(string value, Filters filters)
    {
        if (filters.Raw.Contains("p:"))
            Project = filters.Raw.Split("p:")[1];

        Tags = filters.Tags;
    }

    public override string ToString()
    {
        return $"Query: List all Tasks";
    }
}

public class ListTaskCommandHandler : IRequestHandler<ListTaskRequest>
{
    private readonly IFighterTaskContext _context;

    public ListTaskCommandHandler(IFighterTaskContext context)
    {
        _context = context;
    }

    public Task<Unit> Handle(ListTaskRequest request, CancellationToken cancellationToken)
    {
        List<TodoTask> tasks = _context.Backlog.ToList();
        
        DisplayTodoList(tasks);

        if (tasks.Count < 15)
            return Task.FromResult(new Unit());
        
        string filterOption = AnsiConsole.Ask<string>(
            "(f)ilter or (q)uit");
        
        if (filterOption == "f")
            DisplayInteractiveTodoList(tasks);

        return Task.FromResult(new Unit());
    }

    private void DisplayTodoList(List<TodoTask> tasks)
    {
        var rule = new Rule($"[bold yellow on blue]{_context.Context}[/] [red]BackLog[/]");
        AnsiConsole.Write(rule);

        TaskFighterTable<TodoTask> table = new(tasks, new List<string>()
        {
            "Id",
            "Name",
            "Status",
            "Tags"
        });
        AnsiConsole.Write(table.Table);
    }

    internal class FilterOptions
    {
        public DateTime? Date { get; set; }
        public string Contains { get; set; }
    }

    private void DisplayInteractiveTodoList(List<TodoTask> tasks)
    {
        var filterOptions = new FilterOptions();
        var filteredTasks = tasks; 
        while (true)
        {
            Console.Clear();
            
            DisplayTodoList(filteredTasks);
            
            AnsiConsole.Write(new Rule("[yellow]Filtres de Tâches[/]").RuleStyle("grey"));

            filterOptions.Contains = AnsiConsole.Ask<string>("Search task contains (a for all): ");

            // Filtrer et afficher les tâches
            filteredTasks = (filterOptions.Contains != "a")
                ? tasks.Where(t => t.Name.Contains(filterOptions.Contains)).ToList()
                : tasks;

            DisplayTodoList(filteredTasks);
            
            if (filterOptions.Contains == "q")
                break;
        }
    }
}