using TaskFighter.Domain.Common;
using TaskFighter.Domain.EventsManagement;
using TaskFighter.Domain.TasksManagement;

namespace TaskFighter.Domain;

public interface IFighterTaskContext
{
    public IReadOnlyList<TodoTask> Tasks { get; }
    public IReadOnlyList<TodoEvent> Events { get; }
    public IReadOnlyList<TodoTask> Backlog { get; }
    public DailyTodoLists DailyTodoLists { get; } 
    public DailyTodo DailyTodo { get; }
    
    TodoTask GetTask(int requestTaskId);
    public TodoTask AddTask(TodoTask newTask);

    void DeleteTask(int taskId);
    public void Update(TodoTask task);
    public void Delete(TodoTask task);

    public void TackleToday(TodoTask task);
    public void BackToBacklog(TodoTask returningTask);
    public void Migrate(TodoTask migrateTask);
    
    void AddEvent(TodoEvent todoEvent);
    void UpdateEvent(TodoEvent todoEvent);
    void DeleteEvent(int eventId);
    
    public void SaveChanges();
    public void BackUp();
    void RaiseDomainEvent(BaseEvent<Entity> domainEvent);
}