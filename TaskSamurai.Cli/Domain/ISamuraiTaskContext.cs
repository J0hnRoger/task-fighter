using TaskSamurai.Domain.EventsManagement;
using TaskSamurai.Domain.TasksManagement;

namespace TaskSamurai.Domain;

public interface ISamuraiTaskContext
{
    public IReadOnlyList<TodoTask> Tasks { get; }
    public IReadOnlyList<TodoEvent> Events { get; }

    public TodoTask AddTask(TodoTask newTask);

    void DeleteTask(int taskId);
    public void Update(TodoTask task);
    public void Delete(TodoTask task);
    
    public void SaveChanges();
    public void BackUp();
}