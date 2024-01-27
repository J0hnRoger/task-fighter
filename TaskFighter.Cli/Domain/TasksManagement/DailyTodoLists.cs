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
    
    public DailyTodo? GetTodoList(int todoListId)
    {
       return Days.FirstOrDefault(d => d.Id == todoListId); 
    }
    
    public List<DailyTodo> GetOpenedTodoLists(DateTime from)
    {
       return Days.Where(d => d.IsClosed == false && d.Date > from).ToList(); 
    }
}

public class DailyTodo
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public List<TodoTask> Tasks { get; set; } = new();
    public bool Opened { get; set; }
    public DateTime ClosedDate { get; set; }

    public bool IsClosed => ClosedDate != DateTime.MinValue 
                            && (DateTime.Now > ClosedDate);

    public DailyTodo(int id)
    {
        Id = id;
    }
    
    public List<TodoTask> GetNotFinishedTasks()
    {
        return Tasks.Where(t => t.Status != TodoTaskStatus.Complete).ToList();
    }

    public void Shutdown(DateTime closedDate)
    {
        Opened = false;
        ClosedDate = closedDate;
    }
}