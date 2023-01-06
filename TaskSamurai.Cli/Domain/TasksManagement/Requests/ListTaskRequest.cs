using MediatR;

namespace TaskSamurai.Domain.TasksManagement.Commands;


public record ListTaskRequest : IRequest<List<TodoTask>> 
{
    public ListTaskRequest(string value, string filters)
    {
        Filters = filters;
    }
    
    public override string ToString()
    {
        return $"Query: List all Tasks";
    }

    public string Filters  { get; set; }
}

public class ListTaskCommandHandler : IRequestHandler<ListTaskRequest, List<TodoTask>>
{
    private readonly ISamuraiTaskContext _context;

    public ListTaskCommandHandler(ISamuraiTaskContext context)
    {
        _context = context;
    }
    
   public Task<List<TodoTask>> Handle(ListTaskRequest request, CancellationToken cancellationToken)
   {
       return Task.FromResult(_context.Tasks.ToList()); 
   }
}
