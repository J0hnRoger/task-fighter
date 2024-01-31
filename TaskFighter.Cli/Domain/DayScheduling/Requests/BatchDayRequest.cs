using System.Diagnostics;
using MediatR;
using Spectre.Console;
using TaskFighter.Domain.TasksManagement;
using TaskFighter.Infrastructure.CommandParsing;
using TaskFighter.Infrastructure.Persistence;

namespace TaskFighter.Domain.DayScheduling.Requests;

public record BatchDayRequest : IRequest
{
    public DateTime Date { get; set; }

    public BatchDayRequest(string serializedValue, Filters filters)
    {
        if (string.IsNullOrWhiteSpace(filters.Raw))
            throw new ArgumentException("No date provided for batch day request");
        Date = DateTime.Parse(filters.Raw);
    }
}

public class BatchDayRequestHandler : IRequestHandler<BatchDayRequest>
{
    private readonly IFighterTaskContext _context;
    private readonly string _baseDirectory;

    public BatchDayRequestHandler(IFighterTaskContext context, TaskFighterConfig config)
    {
        _baseDirectory = config.GetBasePath();  
        _context = context;
    }

    public async Task<Unit> Handle(BatchDayRequest request, CancellationToken cancellationToken)
    {
        var todoList = _context.DailyTodoLists.GetTodoList(request.Date);
        if (todoList == null)
            throw new Exception($"No todo list found for {request.Date}");
        
        string batchFile = Path.Combine(_baseDirectory, $"day_{request.Date:dd-MM-yyyy}.md");
        string batchContent = await File.ReadAllTextAsync(batchFile);
        
        var batch = new BatchFile(batchContent);  
        // Deleting the file after the import? 
        return new Unit();
    }
}