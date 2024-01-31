using MediatR;
using TaskFighter.Infrastructure.CommandParsing;
using TaskFighter.Infrastructure.Persistence;

namespace TaskFighter.Domain.TasksManagement.Requests;

// Batch Tasking  
// Allow us to create multiple tasks at once
// Crafting our project
public record BatchTaskRequest : IRequest<List<TodoTask>>
{
    public string BatchFullPath { get; set; }
    
    public BatchTaskRequest(string value, Filters filter)
    {
        BatchFullPath = filter.Raw;
    }
}

public class BatchTaskCommandHandler : IRequestHandler<BatchTaskRequest, List<TodoTask>>
{
    private readonly IFighterTaskContext _context;
    private readonly string _baseDirectory;

    public BatchTaskCommandHandler(IFighterTaskContext context, TaskFighterConfig config)
    {
        _baseDirectory = config.GetBasePath();  
        _context = context;
    }

    public async Task<List<TodoTask>> Handle(BatchTaskRequest request, CancellationToken cancellationToken)
    {
        string batchContent = await File.ReadAllTextAsync(request.BatchFullPath, cancellationToken);
        BatchFile batchFile = new BatchFile(batchContent);

        foreach (TodoTask todo in batchFile.Tasks)
        {
            if (todo.Id == 0)
                _context.AddTask(todo);
            else
            {
                var updatingTaskResult = _context.GetTask(todo.Id);
                if (updatingTaskResult.IsSuccess)
                {
                    var updatingTask = updatingTaskResult.Value;
                    updatingTask.Name = todo.Name;
                    updatingTask.Tags = todo.Tags;
                    // TODO
                } 
            }
        }
        
        _context.SaveChanges();
        return batchFile.Tasks;
    }
}