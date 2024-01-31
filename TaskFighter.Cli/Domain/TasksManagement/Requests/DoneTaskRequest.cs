using MediatR;
using Spectre.Console;
using TaskFighter.Infrastructure.CommandParsing;
using TaskFighter.Infrastructure.Persistence;

namespace TaskFighter.Domain.TasksManagement.Requests;

public class DoneTaskRequest : IRequest<TodoTask>
{
    public int? TaskId  { get; set; }
    
    public DoneTaskRequest(string value, Filters filter)
    {
        if (int.TryParse(filter.Raw, out int taskId))
            TaskId = taskId;
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
       var task = (request.TaskId.HasValue)
           ? _context.DailyTodo.Tasks.Find(t => t.Id == request.TaskId)
           : _context.DailyTodo.Tasks.FirstOrDefault(t => t.Status == TodoTaskStatus.Active);
       
        if (task == null)
            throw new Exception($"Task not found: {request.TaskId}");
        
        task.Finish(DateTime.Now);
        _context.Update(task);
        _context.CompleteTask(task);
        AnsiConsole.Write(new Markup($"[green]Task {task.Id} - {task.Name} Finished[/]"));
        return Task.FromResult(task);
   }
}
