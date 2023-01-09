using MediatR;
using Spectre.Console;
using TaskSamurai.Infrastructure.Persistence;

namespace TaskSamurai.Domain.TasksManagement.Commands;

public record AddTaskRequest : IRequest<TodoTask>
{
    public AddTaskRequest(string serializedValue, string serializedFilters)
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

    public string Context { get; set; }
    public string Name { get; set; }
    public string Area { get; set; }
}

public class CreateTaskCommandHandler : IRequestHandler<AddTaskRequest, TodoTask>
{
    private readonly ISamuraiTaskContext _context;

    public CreateTaskCommandHandler(ISamuraiTaskContext context)
    {
        _context = context;
    }

    public Task<TodoTask> Handle(AddTaskRequest request, CancellationToken cancellationToken)
    {
        var task = new TodoTask()
        {
            Name = request.Name,
            Area = request.Area,
            Context =  request.Context,
            Status = TodoTaskStatus.BackLog
        };

        _context.AddTask(task);
        _context.SaveChanges();
        return Task.FromResult(task);
    }
}