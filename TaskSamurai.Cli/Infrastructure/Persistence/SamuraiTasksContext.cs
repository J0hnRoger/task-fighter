using Newtonsoft.Json;
using TaskSamurai.Domain;
using TaskSamurai.Domain.Common;
using TaskSamurai.Domain.EventsManagement;
using TaskSamurai.Domain.RitualsManagement;
using TaskSamurai.Domain.TasksManagement;

namespace TaskSamurai.Infrastructure.Persistence;

public class SamuraiTasksContext : ISamuraiTaskContext
{
    private readonly TaskSamuraiConfig _dbConfig;

    private readonly List<BaseEvent<Entity>> _domainEvents = new();

    private List<TodoTask> _tasks { get; set; }
    private List<TodoEvent> _events { get; set; }
    private List<Ritual> _rituals { get; set; }

    public IReadOnlyList<TodoTask> Tasks => _tasks;
    public IReadOnlyList<TodoEvent> Events => _events;
    public IReadOnlyList<Ritual> Rituals => _rituals;

    /// <summary>
    /// Unit of Work for recording tasks
    /// </summary>
    /// <param name="todoPath">Json File path</param>
    /// <param name="dbConfig">Config path, for index</param>
    public SamuraiTasksContext(TaskSamuraiConfig dbConfig)
    {
        _dbConfig = dbConfig;

        string fileContent = Load(_dbConfig.TodosPath);
        _tasks = JsonConvert.DeserializeObject<List<TodoTask>>(fileContent) ?? new List<TodoTask>();

        string calendarFileContent = Load(_dbConfig.CalendarPath);
        _events = JsonConvert.DeserializeObject<List<TodoEvent>>(calendarFileContent) ?? new List<TodoEvent>();

        string ritualsFileContent = Load(_dbConfig.RitualsPath);
        _rituals = JsonConvert.DeserializeObject<List<Ritual>>(ritualsFileContent) ?? new List<Ritual>();
    }

    private string Load(string todoPath)
    {
        string fileContent = File.ReadAllText(todoPath);
        return fileContent;
    }

    public void SetContext(string context)
    {
        _dbConfig.Context = context.ToLower();
    }

    public Ritual AddRitual(Ritual newRitual)
    {
        newRitual.Id = ++_dbConfig.CurrentIndex;
        _rituals.Add(newRitual);

        _domainEvents.Add(RitualEvents.CreateRitualEvent(newRitual));
        return newRitual;
    }

    public TodoEvent AddEvent(TodoEvent newTodoEvent)
    {
        newTodoEvent.Id = ++_dbConfig.CurrentIndex;
        _events.Add(newTodoEvent);

        _domainEvents.Add(CalendarEvents.CreateEventEvent(newTodoEvent));
        return newTodoEvent;
    }

    public TodoTask GetTask(int taskId)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == taskId);
        return task;
    }

    public TodoTask AddTask(TodoTask newTask)
    {
        newTask.Id = ++_dbConfig.CurrentIndex;
        if (String.IsNullOrWhiteSpace(newTask.Context))
            newTask.Context = _dbConfig.Context;
        newTask.Created = DateTime.Now;

        _tasks.Add(newTask);
        _domainEvents.Add(TaskEvents.CreateTaskEvent(newTask));
        return newTask;
    }

    public void DeleteTask(int taskId)
    {
        var deletingTask = _tasks.Find(t => t.Id == taskId);
        if (deletingTask == null)
            throw new Exception($"Task Entity {taskId} doesn't exists");
        _tasks.Remove(deletingTask);
        _domainEvents.Add(TaskEvents.DeleteTaskEvent(deletingTask));
    }

    public void Update(TodoTask task)
    {
        _domainEvents.AddRange(task.DomainEvents);
        task.ClearDomainEvents();
    }

    public void Delete(TodoTask task)
    {
        throw new NotImplementedException();
    }

    public void SaveChanges()
    {
        File.WriteAllText(_dbConfig.ConfigPath, JsonConvert.SerializeObject(_dbConfig));

        File.WriteAllText(_dbConfig.TodosPath, JsonConvert.SerializeObject(_tasks));
        File.WriteAllText(_dbConfig.CalendarPath, JsonConvert.SerializeObject(_events));
        File.WriteAllText(_dbConfig.RitualsPath, JsonConvert.SerializeObject(_rituals));

        LogEvents();
    }

    private void LogEvents()
    {
        string content = File.ReadAllText(_dbConfig.LoggerPath);
        List<SerializedEvent> events = JsonConvert.DeserializeObject<List<SerializedEvent>>(content);
        events.AddRange(_domainEvents.Select(d => new SerializedEvent()
        {
            Created = d.Created,
            Entity = d.Entity,
            EntityId = d.EntityId,
            Type = d.Type
        }));
        File.WriteAllText(_dbConfig.LoggerPath, JsonConvert.SerializeObject(events));
    }

    private void SaveChanges(string filePath)
    {
        File.WriteAllText(filePath, JsonConvert.SerializeObject(_tasks, Formatting.Indented));
    }

    public void BackUp()
    {
        string formatDate = DateTime.Today.ToString("dd-MM-yyyy");
        SaveChanges(_dbConfig.TodosPath.Split(".json")[0] + formatDate + ".json");
    }
}