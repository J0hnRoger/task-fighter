using MediatR;

namespace TaskSamurai.Domain.TasksManagement.Commands;

public class FinishTaskRequest : IRequest<TodoTask>
{
    public int TaskId  { get; set; }
    
    public FinishTaskRequest(string value, string filter)
    {
        TaskId = int.Parse(filter);
    }
    
    public override string ToString()
    {
        return $"Command: Task Finished: {TaskId}";
    }
}

public class FinishTaskRequestHandler : IRequestHandler<FinishTaskRequest, TodoTask>
{
    private readonly ISamuraiTaskContext _context;

    public FinishTaskRequestHandler(ISamuraiTaskContext context)
    {
        _context = context;
    }
    
   public Task<TodoTask> Handle(FinishTaskRequest request, CancellationToken cancellationToken)
   {
        var task = _context.GetTask(request.TaskId);
        task.Finish(DateTime.Now);
        _context.Update(task);
        
       _context.SaveChanges();
       return Task.FromResult(task); 
   }
}
