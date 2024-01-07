using MediatR;

namespace TaskFighter.Domain.EventsManagement.Requests;

public record AddEventRequest : IRequest<TodoEvent>
{
    public string Context { get; set; }
    public string Name { get; set; }
    public string Area { get; set; }
    
    public AddEventRequest(string serializedValue, string serializedFilters)
    {
        int nearestModifier = (serializedValue.IndexOf("a:") < serializedValue.IndexOf("c:"))
            ? serializedValue.IndexOf("a:")
            : serializedValue.IndexOf("c:");

        Name = (nearestModifier > -1)
            ? serializedValue.Substring(0, nearestModifier).Trim()
            : serializedValue.Trim();

        Area = serializedValue.Contains("a:")
            ? serializedValue.Split("a:")[1].Split(" ")[0]
            : "work";

        Context = serializedValue.Contains("c:")
            ? serializedValue.Split("c:")[1].Split(" ")[0]
            : "perso";
    }
}

public class AddEventCommandHandler : IRequestHandler<AddEventRequest, TodoEvent>
{
    private readonly IFighterTaskContext _context;

    public AddEventCommandHandler(IFighterTaskContext context)
    {
        _context = context;
    }

    public Task<TodoEvent> Handle(AddEventRequest request, CancellationToken cancellationToken)
    {
        var todoEvent = new TodoEvent()
        {
            Name = request.Name,
        };

        _context.AddEvent(todoEvent);
        _context.SaveChanges();
        return Task.FromResult(todoEvent );
    }
}