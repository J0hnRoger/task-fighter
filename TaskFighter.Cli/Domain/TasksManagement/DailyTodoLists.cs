using TaskFighter.Infrastructure.Persistence;

namespace TaskFighter.Domain.TasksManagement;

public class DailyTodoLists
{
    public List<DailyTodo> Days { get; set; } 
    
    public bool HasOpenedDay => Days
        .Any(d => d.Opened);
    
    public DailyTodo? GetTodoList(DateTime date)
    {
       return Days.FirstOrDefault(d => d.Date == date); 
    }
}

public class DailyTodo
{
    public DateTime Date { get; set; }
    public List<TodoTask> Tasks { get; set; } 
    public bool Opened { get; set; }
    public DateTime ClosedDate { get; set; }

    public List<TodoTask> GetNotFinishedTasks()
    {
        return Tasks.Where(t => t.Status != TodoTaskStatus.Complete).ToList();
    }

    public void Shutdown(DateTime closedDate)
    {
        var notFinishedTasks = Tasks
            .Where(t => t.Status != TodoTaskStatus.Complete)
            .ToList();
        
        if (notFinishedTasks.Any())
            throw new Exception($"Not finished tasks: {string.Join(",", notFinishedTasks.Select(t => t.Id + " - " + t.Name))}");
        
        ClosedDate = closedDate;
    }
}