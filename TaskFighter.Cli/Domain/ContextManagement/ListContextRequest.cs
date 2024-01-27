using MediatR;
using Spectre.Console;
using TaskFighter.Domain.TasksManagement;
using TaskFighter.Infrastructure.CommandParsing;

namespace TaskFighter.Domain.ContextManagement;

public record ListContextRequest : IRequest
{
    public string Name { get; set; }

    public ListContextRequest(string serializedValue, Filters filters)
    {
        Name = serializedValue.Trim();
    }
}

public class ListContextCommandHandler : IRequestHandler<ListContextRequest>
{
    private readonly IFighterTaskContext _context;

    public ListContextCommandHandler(IFighterTaskContext context)
    {
        _context = context;
    }

    public Task<Unit> Handle(ListContextRequest request, CancellationToken cancellationToken)
    {
        DisplayContext(_context.Context);
        return Task.FromResult(new Unit());
    }

    private void DisplayContext(string requestName)
    {
        AnsiConsole.Write($"Context: {requestName}");
    }
}
