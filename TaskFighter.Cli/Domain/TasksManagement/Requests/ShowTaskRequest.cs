using MediatR;
using Spectre.Console;
using TaskFighter.Infrastructure.CommandParsing;
using TaskFighter.Infrastructure.Persistence;

namespace TaskFighter.Domain.TasksManagement.Requests;

public record ShowTaskRequest : IRequest<TodoTask>
{
    public int Id { get; set; }

    public ShowTaskRequest(string serializedValue, Filters filters)
    {
        if (!filters.Id.HasValue)
            throw new ApplicationException("Id is required to show a task");
        Id = filters.Id.Value;
    }
}

public class ShowTaskCommandHandler : IRequestHandler<ShowTaskRequest, TodoTask>
{
    private readonly IFighterTaskContext _context;
    private readonly ITextEditor _editor;
    private readonly string _workingDirPath;

    public ShowTaskCommandHandler(IFighterTaskContext context, ITextEditor editor, TaskFighterConfig config)
    {
        _context = context;
        _workingDirPath = config.GetWorkingDirectory(); 
        _editor = editor;
    }

    public Task<TodoTask> Handle(ShowTaskRequest request, CancellationToken cancellationToken)
    {
        var task = _context.GetTask(request.Id);
        if (task.IsFailure)
            throw new ApplicationException("No task found with this id");
        
        string filePath = _workingDirPath + "/" + task.Value.Id + ".md";
        var batchTask = new BatchFile(new List<TodoTask>() { task.Value });
        File.WriteAllText(filePath, batchTask.ToString());
        
        _editor.Open(filePath);
        
        return Task.FromResult(task.Value);
    }
}