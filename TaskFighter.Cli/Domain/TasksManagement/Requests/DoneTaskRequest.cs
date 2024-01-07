using MediatR;

namespace TaskFighter.Domain.TasksManagement.Requests;

public class DoneTaskRequest : IRequest<TodoTask>
{
    public int TaskId  { get; set; }
    
    public DoneTaskRequest(string value, string filter)
    {
        TaskId = int.Parse(filter);
    }
    
    public override string ToString()
    {
        return $"Command: Task Finished: {TaskId}";
    }
}

public class DoneTaskRequestHandler : IRequestHandler<DoneTaskRequest, TodoTask>
{
    private readonly IFighterTaskContext _context;

    public DoneTaskRequestHandler(IFighterTaskContext context)
    {
        _context = context;
    }
    
   public Task<TodoTask> Handle(DoneTaskRequest request, CancellationToken cancellationToken)
   {
        var task = _context.GetTask(request.TaskId);
        task.Finish(DateTime.Now);
        _context.Update(task);
        
       _context.SaveChanges();
       return Task.FromResult(task); 
   }
}
