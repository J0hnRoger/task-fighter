using Newtonsoft.Json;
using TaskFighter.Domain;
using TaskFighter.Domain.Common;
using TaskFighter.Domain.EventsManagement;
using TaskFighter.Domain.TasksManagement;

namespace TaskFighter.Infrastructure.Persistence;

public class FighterTasksContext : IFighterTaskContext
{
    private readonly TaskFighterConfig _dbConfig;

    private readonly List<BaseEvent<Entity>> _domainEvents = new();
    private readonly DateOnly _today;

    private DailyTodoLists _todoLists { get; set; }
    private List<TodoTask> _backlog { get; set; }
    private List<TodoEvent> _events { get; set; }

    public IReadOnlyList<TodoTask> Backlog => _backlog;
    public IReadOnlyList<TodoTask> Tasks => _todoLists.GetTodoList(_today)!.Tasks;
    public IReadOnlyList<TodoEvent> Events => _events;
    
    public string Context => _dbConfig.Context;

    /// <summary>
    /// Unit of Work for recording tasks
    /// </summary>
    /// <param name="dbConfig">Config path, for index</param>
    public FighterTasksContext(TaskFighterConfig dbConfig)
    {
        _dbConfig = dbConfig;
        
        _today = DateOnly.FromDateTime(DateTime.Today);
        string backlogPath = Load(_dbConfig.BackLogPath);
        string dailyTodoPath = Load(_dbConfig.TodosPath);
        
        _backlog = JsonConvert.DeserializeObject<List<TodoTask>>(backlogPath) ?? new List<TodoTask>();
        
        _todoLists = JsonConvert.DeserializeObject<DailyTodoLists>(dailyTodoPath) ?? new DailyTodoLists();
        var dailyTodo = _todoLists.GetTodoList(_today);
        if (dailyTodo == null)
        {
            dailyTodo = new DailyTodo()
            {
                Date = _today,
                Tasks = new List<TodoTask>()
            };
            _todoLists.Days.Add(dailyTodo);
        }
         
        string calendarFileContent = Load(_dbConfig.CalendarPath);
        _events = JsonConvert.DeserializeObject<List<TodoEvent>>(calendarFileContent) ?? new List<TodoEvent>();
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

    public TodoEvent AddEvent(TodoEvent newTodoEvent)
    {
        newTodoEvent.Id = ++_dbConfig.CurrentIndex;
        _events.Add(newTodoEvent);

        _domainEvents.Add(CalendarEvents.CreateEventEvent(newTodoEvent));
        return newTodoEvent;
    }

    public void UpdateEvent(TodoEvent todoEvent)
    {
        throw new NotImplementedException();
    }

    public void DeleteEvent(int eventId)
    {
        throw new NotImplementedException();
    }

    public TodoTask GetTask(int taskId)
    {
        var task = _backlog.FirstOrDefault(t => t.Id == taskId);
        if (task == null)
            throw new Exception($"Task {taskId} doesn't exists"); 
        return task;
    }

    public TodoTask AddTask(TodoTask newTask)
    {
        newTask.Id = ++_dbConfig.CurrentIndex;
        if (String.IsNullOrWhiteSpace(newTask.Context))
            newTask.Context = _dbConfig.Context;
        newTask.Created = DateTime.Now;

        _backlog.Add(newTask);
        _domainEvents.Add(TaskEvents.CreateTaskEvent(newTask));
        return newTask;
    }

    public void TackleToday(TodoTask task)
    {
        var dailyTodo = _todoLists.GetTodoList(_today);
        task.TackleToday(_today);
        dailyTodo!.Tasks.Add(task);
    }
    
    public void DeleteTask(int taskId)
    {
        var deletingTask = _backlog.Find(t => t.Id == taskId);
        if (deletingTask == null)
            throw new Exception($"Task Entity {taskId} doesn't exists");
        _backlog.Remove(deletingTask);
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

    void IFighterTaskContext.AddEvent(TodoEvent todoEvent)
    {
        throw new NotImplementedException();
    }

    public void SaveChanges()
    {
        File.WriteAllText(_dbConfig.ConfigPath, JsonConvert.SerializeObject(_dbConfig, Formatting.Indented));

        File.WriteAllText(_dbConfig.CalendarPath, JsonConvert.SerializeObject(_events, Formatting.Indented));

        LogEvents();
        
        File.WriteAllText(_dbConfig.BackLogPath, JsonConvert.SerializeObject(_backlog, Formatting.Indented));
        File.WriteAllText(_dbConfig.TodosPath, JsonConvert.SerializeObject(_todoLists, Formatting.Indented));
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
        File.WriteAllText(filePath, JsonConvert.SerializeObject(_todoLists, Formatting.Indented));
    }

    public void BackUp()
    {
        string formatDate = DateTime.Today.ToString("dd-MM-yyyy");
        SaveChanges(_dbConfig.TodosPath.Split(".json")[0] + formatDate + ".json");
    }

    public void RaiseDomainEvent(BaseEvent<Entity> domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}