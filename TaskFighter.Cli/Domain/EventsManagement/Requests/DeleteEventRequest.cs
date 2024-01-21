using MediatR;
using TaskFighter.Infrastructure.CommandParsing;

namespace TaskFighter.Domain.EventsManagement.Requests;


public record DeleteEventRequest : IRequest<Unit> 
{
    public int EventId  { get; set; }
    
    public DeleteEventRequest(string value, Filters filter)
    {
        EventId = int.Parse(filter.Raw);
    }
    
    public override string ToString()
    {
        return $"Command: Delete Event Id {EventId}";
    }
}

public class DeleteEventCommandHandler : IRequestHandler<DeleteEventRequest, Unit>
{
    private readonly IFighterTaskContext _context;

    public DeleteEventCommandHandler(IFighterTaskContext context)
    {
        _context = context;
    }
    
   public Task<Unit> Handle(DeleteEventRequest request, CancellationToken cancellationToken)
   {
       _context.DeleteEvent(request.EventId);
       _context.SaveChanges();
       return Task.FromResult(new Unit()); 
   }
}
