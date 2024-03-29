﻿using System.Diagnostics;
using MediatR;
using TaskFighter.Domain.TasksManagement;
using TaskFighter.Infrastructure.CommandParsing;
using TaskFighter.Infrastructure.Persistence;

namespace TaskFighter.Domain.DayScheduling.Requests;

public record ModifyDayRequest : IRequest
{
    public DateTime Date { get; set; }
    public int? Id { get; set; }

    public ModifyDayRequest(string serializedValue, Filters filters)
    {
        Id = filters.Id;
        Date = DateTime.Today;
    }
}

public class ModifyDayRequestHandler : IRequestHandler<ModifyDayRequest>
{
    private readonly IFighterTaskContext _context;
    private readonly string _baseDirectory;
    private readonly ITextEditor _editor;

    public ModifyDayRequestHandler(IFighterTaskContext context, TaskFighterConfig config, ITextEditor editor)
    {
        _context = context;
        _editor = editor;
        _baseDirectory = config.GetBasePath();  
    }

    public async Task<Unit> Handle(ModifyDayRequest request, CancellationToken cancellationToken)
    {
        var todoList = _context.DailyTodoLists.GetTodoList(request.Date);
        if (todoList == null)
            throw new Exception($"No todo list found for {request.Date}");

        string exportName = todoList.Date.ToString("dd-MM-yyyy");
        BatchFile export = new(todoList.Tasks);     
        string exportPath = $"{_baseDirectory}/day_{exportName}.md";
        
        await File.WriteAllTextAsync(exportPath, export.ToString(), cancellationToken);
        
        _editor.Open(exportPath);
        return new Unit();
    }
}