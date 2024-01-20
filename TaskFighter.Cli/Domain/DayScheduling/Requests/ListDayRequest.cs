﻿using MediatR;
using Spectre.Console;
using TaskFighter.Domain.Common;
using TaskFighter.Domain.TasksManagement;

namespace TaskFighter.Domain.DayScheduling.Requests;
    
public record ListDayRequest : IRequest<Unit>
{
    public DateTime From { get; set; } = DateTime.MinValue;

    public ListDayRequest(string values, string filters)
    {
        if (filters.Contains("week"))
            From = DateUtilities.GetFirstDayOfWeek();
    }
}

public class ListDayRequestHandler : IRequestHandler<ListDayRequest, Unit>
{
    private readonly IFighterTaskContext _context;

    public ListDayRequestHandler(IFighterTaskContext context)
    {
        _context = context;
    }

    public Task<Unit> Handle(ListDayRequest request, CancellationToken cancellationToken)
    {
        var openedTodoLists = _context.DailyTodoLists.GetOpenedTodoLists(request.From);
        
        if (!openedTodoLists.Any())
        {
            AnsiConsole.WriteLine("No Opened TodoLists");
            return Task.FromResult(new Unit());
        }
        
        DisplayTodoLists(openedTodoLists);
        
        return Task.FromResult(new Unit());
    }

    private void DisplayTodoLists(IEnumerable<DailyTodo> todoLists)
    {
        var tree = new Tree("Open Todo Lists");

        foreach (var list in todoLists)
        {
            var node = tree.AddNode(list.Date.ToString("d"));
            foreach (var task in list.Tasks)
            {
                node.AddNode(task.Name);
            }
        }

        AnsiConsole.Write(tree);
    }}