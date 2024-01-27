using MediatR;
using Spectre.Console;
using TaskFighter.Domain.TasksManagement;
using TaskFighter.Infrastructure.CommandParsing;

namespace TaskFighter.Domain.ContextManagement;

public record ModifyContextRequest : IRequest
{
    public string Name { get; set; }

    public ModifyContextRequest(string serializedValue, Filters filters)
    {
        Name = serializedValue.Trim();
    }
}

public class ModifyContextCommandHandler : IRequestHandler<ModifyContextRequest>
{
    private readonly IFighterTaskContext _context;

    public ModifyContextCommandHandler(IFighterTaskContext context)
    {
        _context = context;
    }

    public Task<Unit> Handle(ModifyContextRequest request, CancellationToken cancellationToken)
    {
        _context.SetContext(request.Name);
        
        DisplayContext(request.Name);
        return Task.FromResult(new Unit());
    }

    private void DisplayContext(string requestName)
    {
        AnsiConsole.Write($"Context: {requestName}");
    }
}
