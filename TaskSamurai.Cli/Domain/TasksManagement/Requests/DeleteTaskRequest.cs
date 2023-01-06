﻿using System.Text.Json.Serialization.Metadata;
using MediatR;

namespace TaskSamurai.Domain.TasksManagement.Commands;


public record DeleteTaskRequest : IRequest<TodoTask> 
{
    public int TaskId  { get; set; }
    
    public DeleteTaskRequest(string value, string filter)
    {
        TaskId = int.Parse(filter);
    }
    
    public override string ToString()
    {
        return $"Command: Delete Task Id {TaskId}";
    }
}

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskRequest, TodoTask>
{
    private readonly ISamuraiTaskContext _context;

    public DeleteTaskCommandHandler(ISamuraiTaskContext context)
    {
        _context = context;
    }
    
   public Task<TodoTask> Handle(DeleteTaskRequest request, CancellationToken cancellationToken)
   {
       _context.DeleteTask(request.TaskId);
       _context.SaveChanges();
       return Task.FromResult(new TodoTask()); 
   }
}
