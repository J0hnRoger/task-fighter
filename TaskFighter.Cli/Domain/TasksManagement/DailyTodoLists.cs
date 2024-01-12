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
}