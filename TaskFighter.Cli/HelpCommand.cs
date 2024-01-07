using MediatR;
using TaskFighter.Infrastructure.CommandParsing;

namespace TaskFighter;


public record HelpRequest : IRequest
{
}

public class HelpRequestHandler: IRequestHandler<HelpRequest>
{
    public Task<Unit> Handle(HelpRequest request, CancellationToken cancellationToken)
    {
        Console.WriteLine("TaskFighter is a CLI tool to help you manage your tasks.");
        Console.WriteLine("Pattern: [entity] [filter] [command] [modifications] [miscellaneous]");
        Console.WriteLine($"Regex: {CommandParser.commandParsingRegex}");
        Console.WriteLine("Commands:");
        Console.WriteLine("  add [task name] - Add a new task");
        Console.WriteLine("  list [all|active] - List all tasks or active tasks");
        Console.WriteLine("  start [task id] - Start a task");
        Console.WriteLine("  stop [task id] - Stop a task");
        Console.WriteLine("  complete [task id] - Complete a task");
        Console.WriteLine("  delete [task id] - Delete a task");
        Console.WriteLine("  help - Show help");
        
        return Task.FromResult(new Unit());
    }    
}