using MediatR;
using TaskFighter.Infrastructure.CommandParsing;
using TaskFighter.Infrastructure.Persistence;

namespace TaskFighter.Domain.TasksManagement.Requests;

// Batch Tasking  
// Allow us to create multiple tasks at once
// Crafting our project
public record BatchTaskRequest : IRequest<List<TodoTask>>
{
    public string BatchFile { get; set; }
    
    public BatchTaskRequest(string value, Filters filter)
    {
        BatchFile = value;
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
        string batchFile = Path.Combine(_baseDirectory + "/", request.BatchFile);
        string batchContent = await File.ReadAllTextAsync(batchFile);
        Batch batch = new Batch(batchContent);
        
        foreach (TodoTask todo in batch.Tasks)
            _context.AddTask(todo);
        
        _context.SaveChanges();
        return batch.Tasks;
    }
}