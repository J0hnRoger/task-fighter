namespace TaskFighter.Domain.TasksManagement;

public class DailyTodoLists
{
    public List<DailyTodo> Days { get; set; } = new();

    public DailyTodo? GetTodoList(DateOnly date)
    {
       return Days.FirstOrDefault(d => d.Date == date); 
    }
}

public class DailyTodo
{
    public DateOnly Date { get; set; }
    public List<TodoTask> Tasks { get; set; } = new();
}